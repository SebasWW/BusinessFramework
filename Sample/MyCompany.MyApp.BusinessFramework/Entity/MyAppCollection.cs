using System;
using System.Collections.Generic;
using SebasWW.BusinessFramework;
using SebasWW.BusinessFramework.Factory;

namespace MyCompany.MyApp.Entity
{
    public abstract class MyAppCollection<TObject, TEntry> : BusinessCollection<TObject, TEntry, int>
        where TEntry : class
        where TObject : BusinessObject<TEntry, int>
    {
        internal MyAppCollection(
            BusinessContext BusinessContext, 
            ICollection<TEntry> entrySet, 
            Func<TEntry, int> keySelector,
            BusinessObjectFactory<TObject, TEntry, int> factory
            )
             : base(BusinessContext, entrySet, keySelector, factory) { }
    }
}
