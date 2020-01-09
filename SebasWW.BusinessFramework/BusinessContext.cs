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
    public abstract class BusinessContext : IDisposable
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
        protected BusinessContext(DbContext dbContext, ILogWriter logWriter, IEnumerable<ITracker> trackers)
        {
            DatabaseContext = dbContext;
            LogWriter = logWriter;
            Trackers = trackers;
        }

        protected abstract string GetTableName(Type type);

        public virtual async Task SaveChangesAsync()
        {
            BusinessObjectBase ib;
            var list = new List<TrackingParams>();
            var logs = new List<LogEntry>();

            // Creating list changed objects
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
                                    entityEntry.Metadata.Name,
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

            // Track OnChanging
            if (Trackers != null)
                foreach(var tracker in Trackers)
                    list.ForEach(t => tracker.OnChanging(t));       

            // light check
            list.ForEach(t => (t as IEntryValidator)?.CheckEntry());

            // Transaction ******************************************
            var tran = await DatabaseContext.Database.BeginTransactionAsync(); 

            try
            {
                // precheck in db
                await DoCheckAsync(list.Select(t => t.BusinessObject), true);

                // saving
                await DatabaseContext.SaveChangesAsync(true);

                // save logs
                await LogWriter?.SaveAsync(this, logs);

                // postcheck in db
                await DoCheckAsync(list.Select(t => t.BusinessObject), false);

                //  Track OnChangedUncommitted
                if (Trackers != null)
                    foreach (var tracker in Trackers)
                        list.ForEach(t => tracker.OnChangedUncommitted(t));

                await tran.CommitAsync();

                //  Track OnChangedUncommitted
                if (Trackers != null)
                    foreach (var tracker in Trackers)
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
                            }
                        );
            }
            finally
            {
                if (DatabaseContext.Database.CurrentTransaction?.TransactionId == tran.TransactionId)
                    await tran.RollbackAsync();
            }
        }

        protected virtual async Task DoCheckAsync(IEnumerable<BusinessObjectBase> list, Boolean beforeUpdate)
        {
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
        protected internal virtual TObject CreateBusinessObject<TObject, TEntry, TKey>(BusinessObjectFactory<TObject, TEntry, TKey> factory, TEntry entry)
            where TObject : BusinessObject<TEntry, TKey> 
            where TEntry : class
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (entry is null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

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
