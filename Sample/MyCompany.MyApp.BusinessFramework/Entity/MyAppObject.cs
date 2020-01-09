using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCompany.MyApp.Cache;
using MyCompany.MyApp.Security;
using SebasWW.BusinessFramework;
using SebasWW.BusinessFramework.Security;

namespace MyCompany.MyApp.Entity
{
    public abstract class MyAppObject<TEntry> : BusinessObject<TEntry, int>, IWriteSecurable
        where TEntry : class
    {
        internal MyAppObject(BusinessContext BusinessManager, TEntry entry)
            : base(BusinessManager, entry) { }

        public MyAppObject(TEntry entry) : base(null, entry) { }

        //virtual protected void OnBusinessContextChange() { }
        // secure query
        //protected override IQueryable<TEntry> PermissionRead(IQueryable<TEntry> query) { return query; }

        // ************************************
        // Write Security
        // ************************************
        public virtual async Task<WriteSecurityCheck> CheckSecurityAsQuery(BusinessContext businessManager, bool beforeUpdate)
        {
            var i = (await MyAppCacheManager.GetSecureFeaturesAsync())[SecurityFeatures.EditAll];
            var result = new WriteSecurityCheck();

            if ((await ((MyAppBusinessContext)businessManager).GetUserSecureFeaturesAsync())
                .Where(t => t == i).Any())
            {
                result.State = WriteSecurityState.Allowed;
            }

            return result;
        }
    }
}
