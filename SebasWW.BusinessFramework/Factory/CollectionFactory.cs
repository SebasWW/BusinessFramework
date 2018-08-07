using System;
using System.Collections.Generic;
using System.Text;

namespace SebasWW.BusinessFramework.Factory
{
    public abstract class CollectionFactory<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>
        where TCollection: GenericCollection<TObject, TEntry, TKey>
        where TReadOnlyCollection : GenericReadOnlyCollection<TObject, TEntry, TKey>
        where TObject: GenericObject<TEntry, TKey> 
        where TEntry : class
    {
        protected static object objLock = new object();

        protected abstract TCollection OnCreateInstance(BusinessManager context, ICollection<TEntry> entries);
        internal TCollection CreateInstance(BusinessManager context, ICollection<TEntry> entries)
        {
            return OnCreateInstance(context, entries);
        }

        protected abstract TReadOnlyCollection OnCreateReadOnlyInstance(BusinessManager context, ICollection<TEntry> entries);
        internal TReadOnlyCollection CreateReadOnlyInstance(BusinessManager context, ICollection<TEntry> entries)
        {
            return OnCreateReadOnlyInstance(context, entries);
        }
    }
}
