using System;
using SebasWW.BusinessFramework;
using SebasWW.BusinessFramework.Factory;
using SebasWW.BusinessFramework.Query;
using SebasWW.BusinessFramework.Security;

namespace MyCompany.MyApp.Query
{
    public class MyAppQueryContext<TCollection, TReadOnlyCollection, TObject, TEntry>: 
        BusinessQueryContext<TCollection, TReadOnlyCollection, TObject, TEntry, Int32>

        where TCollection : BusinessCollection<TObject, TEntry, Int32>
        where TReadOnlyCollection : BusinessReadOnlyCollection<TObject, TEntry, Int32>
        where TObject : BusinessObject<TEntry, Int32>
        where TEntry : class
    {
        public MyAppQueryContext
            (
                BusinessContext businessContext,
                BusinessObjectFactory<TObject, TEntry, Int32> objectFactory,
                BusinessCollectionFactory<TCollection, TReadOnlyCollection, TObject, TEntry, Int32> collectionFactory
            )
            : base(businessContext, objectFactory, collectionFactory)
        { }
    }
}
