using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCompany.MyApp.EntityFramework;

namespace MyCompany.MyApp.Cache
{
    static internal class MyAppCacheManager
    {
        //*****************************************
        // Tables
        //*****************************************
        private static ReadOnlyDictionary<String, Int32> _tables;
        internal static async Task<ReadOnlyDictionary<String, Int32>> GetTablesAsync()
        {
            if (_tables == null)
            {
                await RefreshTablesAsync();
            }

            return _tables;
        }
        internal static async Task RefreshTablesAsync()
        {
            using (var dc = new DatabaseContext())
            {
                //var ds = await dc.Set<STable>().AsNoTracking()
                //    .Select(u => new { u.Id, u.Name })
                //    .ToDictionaryAsync(u => u.Name, u => u.Id);

                //_tables = new ReadOnlyDictionary<String, Int32>(ds);
            }
        }
        //*****************************************
        // Tables
        //*****************************************
        private static ReadOnlyDictionary<String, Int32> _secureFeatures;
        internal static async Task<ReadOnlyDictionary<String, Int32>> GetSecureFeaturesAsync()
        {
            if (_secureFeatures == null)
            {
                await RefreshSecureFeaturesAsync();
            }

            return _secureFeatures;
        }
        internal static async Task RefreshSecureFeaturesAsync()
        {
            using (var dc = new DatabaseContext())
            {
                //var ds = await dc.Set<CSecureFeature>().AsNoTracking()
                //    .Select(u => new { u.Id, u.ShortName })
                //    .ToDictionaryAsync(u => u.ShortName, u => u.Id);

                //_secureFeatures = new ReadOnlyDictionary<String, Int32>(ds);
            }
        }


        //*****************************************
        // User SecureFeature
        //*****************************************
        //private static ReadOnlyDictionary<Int32, UserSecureFeatureCache> _userSecureFeatures;
        //internal static ReadOnlyDictionary<int, UserSecureFeatureCache> UserSecureFeatures { get => _userSecureFeatures; }
        //internal static async Task RefreshUserSecureFeaturesAsync()
        //{
        //    using (var dc = new DatabaseContext())
        //    {
        //        var user = await dc.Set<SUser>().AsNoTracking()
        //            .Where(u => u.UserStatusId == 2)
        //            .Select(u => new
        //            {
        //                UserId = u.Id,
        //                SecureFeatures = dc.Set<CSecureFeature>().AsNoTracking()
        //                  .Where(s => s.SRoleSecureFeature.Where(rsf => rsf.Role.SUserRole.Where(ur => ur.UserId == u.Id).Any()).Any())
        //                  .Select(x => new SecureFeatureCache(x.Id, x.ShortName))
        //                  .ToArray()
        //            })
        //            .ToDictionaryAsync(u => u.UserId, u => new UserSecureFeatureCache(u.UserId, u.SecureFeatures));

        //        _userSecureFeatures = new ReadOnlyDictionary<int, UserSecureFeatureCache>(user);
        //    }
        //}

        internal static Task RefreshAll()
        {
            var t1 = RefreshSecureFeaturesAsync();
            var t2 = RefreshTablesAsync();

            return Task.WhenAny(t1, t2);
        }
    }
}
