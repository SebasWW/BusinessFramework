using System;
using System.Collections.Generic;
using System.Text;
using SebasWW.BusinessFramework.Factory;
using SebasWW.BusinessFramework.Security;

namespace SebasWW.BusinessFramework.Query
{
    public class BusinessQueryContext<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>
        where TCollection : GenericCollection<TObject, TEntry, TKey>
        where TReadOnlyCollection : GenericReadOnlyCollection<TObject, TEntry, TKey>
        where TObject : GenericObject<TEntry, TKey>
        where TEntry : class
    {
        internal BusinessManager BusinessContext;
        internal ObjectFactory<TObject, TEntry, TKey> ObjectFactory;
        internal CollectionFactory<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> CollectionFactory;

        internal Boolean IsSecured;
        internal ObjectSecurity<TEntry> ObjectSecurity;

        public BusinessQueryContext
            (
                BusinessManager businessContext,
                ObjectFactory<TObject, TEntry, TKey> objectFactory,
                CollectionFactory<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> collectionFactory
            )
        {
            BusinessContext = businessContext;
            ObjectFactory = objectFactory;
            CollectionFactory = collectionFactory;
            IsSecured = true;
        }

        public BusinessQueryContext
            (
                BusinessManager businessContext,
                ObjectFactory<TObject, TEntry, TKey> objectFactory,
                CollectionFactory<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> collectionFactory,
                ObjectSecurity<TEntry> objectSecurity
            )
        {
            BusinessContext = businessContext;
            ObjectFactory = objectFactory;
            CollectionFactory = collectionFactory;
            ObjectSecurity = objectSecurity;
        }
    }
}
