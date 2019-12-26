using System;
using System.Collections.Generic;
using System.Text;

namespace SebasWW.BusinessFramework.Authentification
{
    public class AuthentificationException : BusinessException
    {
        public AuthentificationException(String message) : base(message){ }

        public AuthentificationException() : base("Authentification is failed.") { }
    }
}
