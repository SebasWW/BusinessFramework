using System;
using System.Collections.Generic;
using System.Text;

namespace Tnomer.BusinessFramework
{
    public abstract class GenericFactory<TObject, TEntry, TKey>
        where TObject: GenericObject<TEntry, TKey> 
        where TEntry : class
    {
        protected static object objLock = new object();

        protected abstract TObject OnCreateInstance(BusinessContext context, TEntry entry);
        internal TObject CreateObjectInstance(BusinessContext context, TEntry entry)
        {
            return OnCreateInstance(context, entry);
        }
    }
}
