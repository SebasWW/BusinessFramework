using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyCompany.MyApp.Entity;
using MyCompany.MyApp.EntityFramework;
using MyCompany.MyApp.EntityFramework.Entity;
using MyCompany.MyApp.Factory;
using MyCompany.MyApp.Log;
using MyCompany.MyApp.Query;
using MyCompany.MyApp.Security;
using SebasWW.BusinessFramework;
using SebasWW.BusinessFramework.Authentification;
using SebasWW.BusinessFramework.Factory;
using SebasWW.BusinessFramework.Query;

namespace MyCompany.MyApp
{
    public partial class MyAppBusinessContext : BusinessContext
    {
        public const string READ_SECURITY_KEY = "MyCompany.MyApp.ReadSecure";

        public MyAppBusinessContext(AuthentificatedUser user):base(new DatabaseContext(), new LogWriter(), null)
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

        //**********************************************
        // Order
        //**********************************************
        
        static readonly Dictionary<object, IBusinessQueryFilter<OrderCollection, OrderReadOnlyCollection, Order, DTOrder, int>> OrderFilters =
            (
                new KeyValuePair<object, IBusinessQueryFilter<OrderCollection, OrderReadOnlyCollection, Order, DTOrder, int>>[] 
                {
                    new KeyValuePair<object, IBusinessQueryFilter<OrderCollection, OrderReadOnlyCollection, Order, DTOrder, int>>(
                        READ_SECURITY_KEY,
                        new CustomBusinessQueryFilter<OrderCollection, OrderReadOnlyCollection, Order, DTOrder, int>(ReadSecurityQuery.Order)
                    )
                }
            )
            .ToDictionary(t => t.Key, v => v.Value);

        public BusinessQuery<OrderCollection, OrderReadOnlyCollection, Order, DTOrder, int> Orders
        {
            get =>
                new BusinessQuery<OrderCollection, OrderReadOnlyCollection, Order, DTOrder, int>(
                    new BusinessQueryContext<OrderCollection, OrderReadOnlyCollection, Order, DTOrder, int>(
                        this,
                        OrderFactory.Current,
                        OrderCollectionFactory.Current,
                        OrderFilters,
                        OrderFilters[READ_SECURITY_KEY]
                    ),
                    DatabaseContext.Set<DTOrder>()
                );
        }
    }
}
