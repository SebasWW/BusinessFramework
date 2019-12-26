using System;

namespace SebasWW.BusinessFramework.Authentification
{
    public class AuthentificatedUser 
    {
        internal AuthentificatedUser(Int32 id, String name)
        {
            Id = id;
            Name = name;
        }

        public Int32 Id { get; private set; }
        public String Name { get; private set; }
    }
}
