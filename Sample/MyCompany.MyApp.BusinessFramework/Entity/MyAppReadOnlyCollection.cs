using System;
using System.Collections.Generic;
using System.Text;
using SebasWW.BusinessFramework;
using SebasWW.BusinessFramework.Factory;

namespace MyCompany.MyApp.Entity
{
    public abstract class MyAppReadOnlyCollection<TObject, TEntry> : BusinessReadOnlyCollection<TObject, TEntry, Int32>
        where TEntry : class
        where TObject : BusinessObject<TEntry, Int32>
    {
        internal MyAppReadOnlyCollection(BusinessManager BusinessContext, ICollection<TEntry> entrySet, Func<TEntry, Int32> keySelector, BusinessObjectFactory<TObject, TEntry, Int32> factory)
             : base(BusinessContext, entrySet, keySelector, factory) { }
    }
}
