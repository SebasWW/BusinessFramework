using System;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Tnomer.AspNetCore.Mvc.Configuration;

namespace Tnomer.AspNetCore.Mvc.Authentication
{
    public static class JwtManager
    {

        public static ClaimsIdentity GetIdentity(String jwtToken)
        {

            JwtConfiguration jwtConfig = ApplicationConfiguration.Current.Jwt;

            var tokenValidationParams = new TokenValidationParameters();

            tokenValidationParams.ValidateAudience = true;
            tokenValidationParams.ValidAudience = jwtConfig.Audience;

            tokenValidationParams.ValidateIssuer = true;
            tokenValidationParams.ValidIssuer = jwtConfig.Issuer;

            tokenValidationParams.ValidateIssuerSigningKey = true;
            tokenValidationParams.IssuerSigningKey = jwtConfig.SigningCredentials().Key;

            SecurityToken secureToken;

            var handler = new JwtSecurityTokenHandler();
            var user = handler.ValidateToken(jwtToken, tokenValidationParams, out secureToken);

            return ((ClaimsIdentity) user.Identity) ;
        }

        public static JwtDescription GenerateToken(ClaimsIdentity identity)
        {
            JwtConfiguration jwtConfig = ApplicationConfiguration.Current.Jwt;

            var now = DateTime.UtcNow;
            var expire = now.Add(TimeSpan.FromMinutes(jwtConfig.LifetimeInMinutes));

            // создаем JWT-токен
            var jwt = new JwtSecurityToken
                (
                    issuer: jwtConfig.Issuer,
                    audience: jwtConfig.Audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: expire,
                    signingCredentials: jwtConfig.SigningCredentials()
                );

            JwtDescription desc = new JwtDescription();

            desc.Token = new JwtSecurityTokenHandler().WriteToken(jwt);
            desc.ExpireDate = expire;

            return desc;
        }
    }

    public class JwtDescription
    {
        public String Token { internal set; get; }
        public DateTime ExpireDate { internal set; get; }

    }
}

