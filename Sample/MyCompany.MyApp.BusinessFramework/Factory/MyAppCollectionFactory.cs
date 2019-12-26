using System;
using SebasWW.BusinessFramework;
using SebasWW.BusinessFramework.Factory;

namespace MyCompany.MyApp.Factory
{
    public abstract class MyAppCollectionFactory<TCollection, TReadOnlyCollection, TObject,TEntry> 
        : BusinessCollectionFactory<TCollection, TReadOnlyCollection, TObject, TEntry, Int32>
        where TCollection : BusinessCollection<TObject, TEntry, Int32>
        where TReadOnlyCollection : BusinessReadOnlyCollection<TObject, TEntry, Int32>
        where TObject : BusinessObject<TEntry, Int32>
        where TEntry : class
    {
    }
}
