using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SebasWW.BusinessFramework.Factory;
using SebasWW.BusinessFramework.Security;

namespace SebasWW.BusinessFramework.Query
{
    public class BusinessQueryContext<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>
        where TCollection : BusinessCollection<TObject, TEntry, TKey>
        where TReadOnlyCollection : BusinessReadOnlyCollection<TObject, TEntry, TKey>
        where TObject : BusinessObject<TEntry, TKey>
        where TEntry : class
    {

        internal readonly BusinessObjectFactory<TObject, TEntry, TKey> ObjectFactory;
        internal readonly BusinessCollectionFactory<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> CollectionFactory;
        internal readonly IImmutableDictionary<object, Func<IQueryable<TEntry>, BusinessQueryContext<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>, IQueryable<TEntry>>> CustomFunctions;
        internal readonly Func<IQueryable<TEntry>, BusinessQueryContext<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>, IQueryable<TEntry>> FinalizingFunction;

        public BusinessQueryContext
            (
                BusinessContext businessContext,
                BusinessObjectFactory<TObject, TEntry, TKey> objectFactory,
                BusinessCollectionFactory<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> collectionFactory,
                IDictionary<object, Func<IQueryable<TEntry>, BusinessQueryContext<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>, IQueryable<TEntry>>> customFunctions = null,
                Func<IQueryable<TEntry>, BusinessQueryContext<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>, IQueryable<TEntry>> finalizingFunction = null
            )
        {
            BusinessContext = businessContext;
            ObjectFactory = objectFactory;
            CollectionFactory = collectionFactory;
            CustomFunctions = customFunctions.ToImmutableDictionary();
            FinalizingFunction = finalizingFunction;
        }

        public Dictionary<object, object> Properties { get; } = new Dictionary<object, object>();

        public BusinessContext BusinessContext { get; }
    }
}
