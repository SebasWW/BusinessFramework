using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.MyApp.Cache
{
    internal class TableEntryCache
    {
        Int32 _id;
        String _name;

        internal TableEntryCache(Int32 id, String name)
        {
            _id = id;
            _name = name;
        }

        internal int Id { get => _id; }
        internal string Name { get => _name; }
    }
}
