using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SebasWW.BusinessFramework;
using SebasWW.BusinessFramework.Factory;

namespace MyCompany.MyApp
{
    public partial class MyAppManager : BusinessManager
    {
        Object objLock = new Object();

        public MyAppManager(AuthentificatedUser user):base(new DatabaseContext(), new LogWriter(), null)
        {
            User = user;
        }

        internal  DbContext DbContext => DatabaseContext;


        //*****************************************
        // context overrides
        //*****************************************
        protected override string GetTableName(Type type)
        {
            return DbContext.Model.GetEntityTypes(type).First().SqlServer().TableName;
        }

        //*****************************************
        // Current user
        //*****************************************
        internal AuthentificatedUser User { get; set; }

        //*****************************************
        // UserSecureFeature cache
        //*****************************************
        private HashSet<Int32> _userSecureFeatures;
        internal async Task<HashSet<Int32>> GetUserSecureFeaturesAsync()
        {
            if (_userSecureFeatures == null)
            {
                await RefreshUserSecureFeaturesAsync();
            }

            return _userSecureFeatures;
        }
        internal async Task RefreshUserSecureFeaturesAsync()
        {
            var user = await DatabaseContext.Set<SRoleSecureFeature>().AsNoTracking()
                .Where(rsf => rsf.Role.SUserRole.Where(ur => ur.UserId == User.Id).Any())
                .Select(x => x.SecureFeatureId)
                .ToArrayAsync();
            
                _userSecureFeatures = new HashSet<Int32>(user);
        }

        internal TObject CreateObject<TObject, TEntry, TKey>(BusinessObjectFactory<TObject, TEntry, TKey> factory, TEntry entry)
            where TObject : BusinessObject<TEntry, TKey>
            where TEntry : class
        {
            return CreateBusinessObject(factory, entry);
        }
    }
}
