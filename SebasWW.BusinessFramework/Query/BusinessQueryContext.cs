using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SebasWW.BusinessFramework.Factory;

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
        internal readonly IImmutableDictionary<object, IBusinessQueryFilter<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>> CustomFilters;
        internal readonly IBusinessQueryFilter<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> FinalizingFilter;

        public BusinessQueryContext
            (
                BusinessContext businessContext,
                BusinessObjectFactory<TObject, TEntry, TKey> objectFactory,
                BusinessCollectionFactory<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> collectionFactory,
                IDictionary<object, IBusinessQueryFilter<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>> customFilters = null,
                IBusinessQueryFilter<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> finalizingFilter = null
            )
        {
            BusinessContext = businessContext;
            ObjectFactory = objectFactory;
            CollectionFactory = collectionFactory;
            CustomFilters = customFilters.ToImmutableDictionary();
            FinalizingFilter = finalizingFilter;
        }

        public Dictionary<object, object> Properties { get; } = new Dictionary<object, object>();

        public BusinessContext BusinessContext { get; }
    }
}
