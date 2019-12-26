using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.MyApp.Cache
{
    internal class SecureFeatureCache
    {
        Int32 _id;
        String _key;

        public SecureFeatureCache(Int32 id, String key)
        {
            _id = id;
            _key = key;
        }

        internal int Id { get => _id; }
        internal string Key { get => _key;}
    }
}
