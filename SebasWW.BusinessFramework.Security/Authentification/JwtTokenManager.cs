using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SebasWW.BusinessFramework.EntityFramework;
using Tnomer.AspNetCore.Mvc.Configuration;
using Tnomer.AspNetCore.Mvc.Authentication;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace SebasWW.BusinessFramework.Authentification
{
    public static class JwtTokenManager
    {
        const String Id = "SebasWW.BusinessFramework.user_id";
        const String Name = "SebasWW.BusinessFramework.user_name";

        public static AuthentificatedUser SignIn(String jwtToken)
        {
			//var identity = JwtManager.GetIdentity(jwtToken);

			//Int32 id = Int32.Parse(identity.Claims.Where(t => t.Type == Id).FirstOrDefault().Value);
			//String name = identity.Claims.Where(t => t.Type == Name).FirstOrDefault().Value;

			//return new AuthentificatedUser(id, name);
			// Борис Николаевич, убрали аутонтификуацию, чтоб было па-проще.
			return new AuthentificatedUser(30, "kaka");
		}

        public static JwtDescription GenerateToken(AuthentificatedUser user)
        {
            var claims = new List<Claim>
                {
                    new Claim(Id, user.Id.ToString()),
                    new Claim(Name, user.Name)
                };
            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            JwtDescription desc = JwtManager.GenerateToken(claimsIdentity);

            return desc;
        }

        //public static async Task<JwtDescription> GenerateToken(IIdentity identity)
        //{
        //    using (DatabaseContext dc = new DatabaseContext())
        //    {
        //        var user = await dc.Set<SUser>().AsNoTracking().Where(
        //            u => (u.UserStatusId == 1 || u.UserStatusId == 0) 
        //                    && u.SUserLogin.Any(ul => ul.Login == identity.Name && ul.ProviderName == identity.AuthenticationType)
        //            ).FirstOrDefaultAsync();

        //        if (user == null) throw new AuthentificationException();

        //        return GenerateToken(new AuthentificatedUser(user.Id, user.FirstName + " " + user.LastName)) ;
        //    }
        //}


    }
}
