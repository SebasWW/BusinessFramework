using System;
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
        internal BusinessManager BusinessContext;
        internal BusinessObjectFactory<TObject, TEntry, TKey> ObjectFactory;
        internal BusinessCollectionFactory<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> CollectionFactory;

        internal Boolean IsSecured;
        internal ReadSecurityFilter<TEntry> ObjectSecurity;

        public BusinessQueryContext
            (
                BusinessManager businessContext,
                BusinessObjectFactory<TObject, TEntry, TKey> objectFactory,
                BusinessCollectionFactory<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> collectionFactory
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
                BusinessObjectFactory<TObject, TEntry, TKey> objectFactory,
                BusinessCollectionFactory<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> collectionFactory,
                ReadSecurityFilter<TEntry> objectSecurity
            )
        {
            BusinessContext = businessContext;
            ObjectFactory = objectFactory;
            CollectionFactory = collectionFactory;
            ObjectSecurity = objectSecurity;
        }
    }
}
