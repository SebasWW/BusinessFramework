using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.MyApp.Cache
{
    internal class UserSecureFeatureCache
    {
        private IEnumerable<SecureFeatureCache> _secureFeatures;
        private Int32 _userId;

        public UserSecureFeatureCache(Int32 user, IEnumerable<SecureFeatureCache> secureFeatures)
        {
            _userId = user;
            _secureFeatures = secureFeatures;
        }

        public int UserId { get => _userId;  }
        internal IEnumerable<SecureFeatureCache> SecureFeatures { get => _secureFeatures;}
    }
}
