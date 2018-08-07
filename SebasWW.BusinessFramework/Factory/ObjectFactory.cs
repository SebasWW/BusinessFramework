using System;
using System.Collections.Generic;
using System.Text;

namespace SebasWW.BusinessFramework.Factory
{
    public abstract class ObjectFactory<TObject, TEntry, TKey>
        where TObject: GenericObject<TEntry, TKey> 
        where TEntry : class
    {
        protected static object objLock = new object();

        protected abstract TObject OnCreateInstance(BusinessManager context, TEntry entry);
        internal TObject CreateInstance(BusinessManager context, TEntry entry)
        {
            return OnCreateInstance(context, entry);
        }
    }
}
