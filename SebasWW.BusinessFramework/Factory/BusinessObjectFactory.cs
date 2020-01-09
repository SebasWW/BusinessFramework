using System;
using System.Collections.Generic;
using System.Text;

namespace SebasWW.BusinessFramework.Factory
{
    public abstract class BusinessObjectFactory<TObject, TEntry, TKey>
        where TObject: BusinessObject<TEntry, TKey> 
        where TEntry : class
    {
        protected static object objLock = new object();

        protected abstract TObject OnCreateInstance(BusinessContext context, TEntry entry);
        internal TObject CreateInstance(BusinessContext context, TEntry entry)
        {
            return OnCreateInstance(context, entry);
        }
    }
}
