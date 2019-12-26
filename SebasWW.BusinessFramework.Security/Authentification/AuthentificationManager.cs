using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SebasWW.BusinessFramework.EntityFramework;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace SebasWW.BusinessFramework.Authentification
{
    public static class AuthentificationManager
    {
        public static async Task<UserInfo> GetUserInfoAsync(IIdentity identity)
        {
            using (DatabaseContext dc = new DatabaseContext())
            {
                var user = await dc.Set<SUser>().AsNoTracking().Where(
                    u => u.SUserLogin.Any(ul => ul.Login == identity.Name && ul.ProviderName == identity.AuthenticationType)).FirstOrDefaultAsync();

                if (user == null) return null;

                return new UserInfo(user);
            }
        }

        public static async Task<AuthentificatedUser> SignInAsync(IIdentity identity)
        {
            using (DatabaseContext dc = new DatabaseContext()) {
                SUser user = await dc.Set<SUser>().AsNoTracking().Where(
                    u => u.UserStatusId == (int)User.UserStatusEnum.Approoved && u.SUserLogin.Any(ul => ul.Login == identity.Name && ul.ProviderName == identity.AuthenticationType )).FirstOrDefaultAsync();

                if (user == null) throw new AuthentificationException();

                return new AuthentificatedUser( user.Id, user.FirstName + " " + user.LastName); 
            }
        }
    }
}
