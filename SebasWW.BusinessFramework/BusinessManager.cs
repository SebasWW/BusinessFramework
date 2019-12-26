using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SebasWW.BusinessFramework.Factory;
using SebasWW.BusinessFramework.Log;
using SebasWW.BusinessFramework.Security;
using SebasWW.BusinessFramework.Tracking;

namespace SebasWW.BusinessFramework
{
    public abstract class BusinessManager : IDisposable
    {
        //******************************************************
        // properties
        //******************************************************
        protected internal virtual DbContext DatabaseContext { get;}
        protected internal ILogWriter LogWriter { get; set; }
        IEnumerable<ITracker> Trackers { get; }

        //******************************************************
        // Constructor
        //******************************************************
        protected BusinessManager(DbContext dbContext, ILogWriter logWriter, IEnumerable<ITracker> trackers)
        {
            DatabaseContext = dbContext;
            LogWriter = logWriter;
            Trackers = trackers ?? new List<ITracker>();
        }

        protected abstract string GetTableName(Type type);

        public virtual async Task SaveChangesAsync()
        {
            BusinessObjectBase ib;
            var list = new List<TrackingParams>();
            var logs = new List<LogEntry>();

            //get list objects
            foreach (var entityEntry in DatabaseContext.ChangeTracker.Entries()
                .Where(t => t.State == EntityState.Added || t.State == EntityState.Modified))
            {
                if (_entries.TryGetValue(entityEntry.Entity, out ib) )
                {
                    list.Add(new TrackingParams(this, ib, entityEntry));

                    // add to log`
                    foreach(var prop in entityEntry.Properties)
                    {
                        if (prop.IsModified || entityEntry.State == EntityState.Added)
                        {
                            var le = new LogEntry(
                                    //entry.Entity,
                                    GetTableName(entityEntry.Entity.GetType()),  // entry.Metadata.Name,
                                    () => entityEntry.CurrentValues[entityEntry.Metadata.FindPrimaryKey()?.Properties.FirstOrDefault()?.Name],
                                    prop.Metadata.Name,
                                    prop.OriginalValue,
                                    () =>  entityEntry.CurrentValues[prop.Metadata.Name]
                                );
                            logs.Add(le);
                        }
                    }
                }
            }

            // no changes
            if (list.Count == 0 )  return;

            //  Track OnChanging
            foreach(var tracker in Trackers)
            {
                list.ForEach(t => tracker.OnChanging(t));
            }            

            //  light check
            list.ForEach(t => (t as IEntryValidator)?.CheckEntry());

            //  Transaction ******************************************
            var tran = await DatabaseContext.Database.BeginTransactionAsync(); 
            try
            {
                //  precheck in db
                await DoCheck(list.Select(t => t.BusinessObject), true);

                //  save!!!
                await DatabaseContext.SaveChangesAsync(true);

                //  save logs
                await LogWriter?.Save(this, logs);

                //  postcheck in db
                await DoCheck(list.Select(t => t.BusinessObject), false);

                //  Track OnChangedUncommitted
                foreach (var tracker in Trackers)
                {
                    list.ForEach(t => tracker.OnChangedUncommitted(t));
                }

                tran.Commit();

                //  Track OnChangedUncommitted
                foreach (var tracker in Trackers)
                {
                    list.ForEach(t =>
                    {
                        try
                        {
                            tracker.OnChanged(t);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Event Changed: " + t.ToString() + Environment.NewLine + ex.ToString());
                        }
                    });
                }
            }
            finally
            {
                if (DatabaseContext.Database.CurrentTransaction?.TransactionId == tran.TransactionId)
                    tran.Rollback();
            }
        }

        async Task DoCheck(IEnumerable<BusinessObjectBase> list, Boolean beforeUpdate)
        {
            //**********************************************
            // WriteSecure
            //**********************************************
            IQueryable<SecurityErrorEntry> secureQuery = null;
            WriteSecurityCheck currentSecureQuery;

            foreach (var obj in list.Where(t => t is IWriteSecurable))
            {
                currentSecureQuery = await (obj as IWriteSecurable).CheckSecurityAsQuery(true);

                if (currentSecureQuery.State != WriteSecurityState.Allowed)
                {
                    // no access
                    if (currentSecureQuery.State == WriteSecurityState.NoAccess)
                        throw new WriteSecurityException(0, "ACCESS_DENIED", obj.GetType().Name);

                    if (currentSecureQuery.Queue == null) throw new BusinessException("WriteSecurityCheck with state 'NeedExecute' must have not nullable 'Query' property.");

                    // need check
                    if (secureQuery == null)
                        secureQuery = currentSecureQuery.Queue;
                    else
                        secureQuery = secureQuery.Union(currentSecureQuery.Queue);
                }
            }
            // db request
            if (secureQuery != null)
            {
                var entry = (await secureQuery.ToArrayAsync()).FirstOrDefault();
                if (entry != null)
                {
                    throw new WriteSecurityException(entry);
                }
            }

            //**********************************************
            // Data Consistency
            //**********************************************
            IQueryable<ConsistencyErrorEntry> checkQuery = null, currentCheckQuery;
            foreach (var obj in list.Where(t => t is IConsistencyValidator))
            {
                currentCheckQuery = (obj as IConsistencyValidator).CheckAsQuery(true);
                if (currentCheckQuery != null)
                {
                    if (checkQuery == null) checkQuery = currentCheckQuery;
                    else checkQuery = checkQuery.Union(currentCheckQuery);
                }
            }
            // db request
            if (checkQuery != null)
            {
                var entry = (await checkQuery.ToArrayAsync()).FirstOrDefault();
                if (entry != null)
                {
                    throw new ConsistencyValidationException(entry);
                }
            }
        }

        ConcurrentDictionary<object, BusinessObjectBase> _entries = new ConcurrentDictionary<object, BusinessObjectBase>();

        // create object from entity
        protected internal TObject CreateBusinessObject<TObject, TEntry, TKey>(BusinessObjectFactory< TObject, TEntry, TKey> factory, TEntry entry)
            where TObject : BusinessObject<TEntry, TKey> 
            where TEntry : class
        {
            return (TObject)_entries.GetOrAdd(entry, k => factory.CreateInstance(this, entry));
        }

        internal void AddBusinessObject<TEntry, TKey>(BusinessObject<TEntry, TKey> obj, bool addEntryToEntrySet) 
            where TEntry : class
        {
            obj.BusinessManager = this;
            if(addEntryToEntrySet) DatabaseContext.Set<TEntry>().Add(obj.Entry);
        }

        // register business object object
        internal void RegisterBusinessObject<TEntry, TKey>(BusinessObject<TEntry, TKey> obj)
            where TEntry : class
        {
            _entries.AddOrUpdate(obj.Entry, obj, (e, old) => obj);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DatabaseContext.Dispose();
                }

                disposedValue = true;
            }
        }

        // ~DatabaseSession() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
