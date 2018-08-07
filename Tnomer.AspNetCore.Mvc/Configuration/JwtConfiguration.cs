using System;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Tnomer.AspNetCore.Mvc.Configuration
{
    public class JwtConfiguration
    {
        public string Issuer { get; set; } // = "Tnomer.Identity"; // издатель токена
        public string Audience { get; set; } // = "http://localhost:51884/"; // потребитель токена
        public int LifetimeInMinutes { get; set; } // = 1; // время жизни токена - 1 минута

        public string Key { get; set; } // = "mysupersecret_secretkey!123";   // ключ для шифрации

        public SigningCredentials SigningCredentials()
        {
                return new SigningCredentials
                        (
                            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key)),
                            SecurityAlgorithms.HmacSha256
                        );
        }   
    }
}
