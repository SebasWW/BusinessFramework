using System;
using System.Security.Principal;

namespace SebasWW.BusinessFramework.Authentification
{
    public class AuthentificatedUser : IIdentity
    {
        public  AuthentificatedUser(string name, string authenticationType)
        {
            Name = name;
            AuthenticationType = authenticationType;
        }

        public string Name { get; }

        public string AuthenticationType { get; }

        public bool IsAuthenticated => true;
    }
}
