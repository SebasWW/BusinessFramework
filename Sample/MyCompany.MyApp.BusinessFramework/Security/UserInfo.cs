using System;
using SebasWW.BusinessFramework.EntityFramework;

namespace SebasWW.BusinessFramework.Authentification
{
    public class UserInfo
    {
        internal UserInfo(SUser user)
        {
            Id = user.Id;
            UserStatusId = user.UserStatusId;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }

        public Int32 Id { get; private set; }
        public Int32 UserStatusId { get; private set; }
        public String FirstName { get; private set; }
        public String LastName { get; private set; }
    }
}
