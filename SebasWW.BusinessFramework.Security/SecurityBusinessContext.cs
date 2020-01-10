using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SebasWW.BusinessFramework.Authentification;
using SebasWW.BusinessFramework.Factory;
using SebasWW.BusinessFramework.Log;
using SebasWW.BusinessFramework.Security;
using SebasWW.BusinessFramework.Tracking;

namespace SebasWW.BusinessFramework
{
    public abstract class SecurityBusinessContext : BusinessContext
    {
        //******************************************************
        // properties
        //******************************************************
        protected override DbContext DatabaseContext { get => base.DatabaseContext; }

        public IIdentity User { get; }

        //******************************************************
        // Constructor
        //******************************************************
        protected SecurityBusinessContext(IIdentity user, DbContext dbContext, ILogWriter logWriter, IEnumerable<ITracker> trackers) 
            :base(dbContext, logWriter, trackers)
        {
            User = user;
        }

        protected override async Task DoCheckAsync(IEnumerable<BusinessObjectBase> list, bool beforeUpdate)
        {
            //**********************************************
            // WriteSecure
            //**********************************************
            IQueryable<SecurityErrorEntry> secureQuery = null;
            WriteSecurityCheck currentSecureQuery;

            foreach (var obj in list.Where(t => t is IWriteSecurable))
            {
                currentSecureQuery = await (obj as IWriteSecurable).CheckSecurityAsQuery(this, true);

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
            await base.DoCheckAsync(list, beforeUpdate);
        }

        ConcurrentDictionary<object, BusinessObjectBase> _entries = new ConcurrentDictionary<object, BusinessObjectBase>();

        // create object from entity
        protected override TObject CreateBusinessObject<TObject, TEntry, TKey>(BusinessObjectFactory<TObject, TEntry, TKey> factory, TEntry entry)
        {
            return base.CreateBusinessObject(factory, entry);
        }

    }
}
