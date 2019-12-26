//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace SebasWW.BusinessFramework.Factory
//{
//    public abstract class BusinessReadOnlyCollectionFactory<TReadOnlyCollection, TObject, TEntry, TKey>
//        where TReadOnlyCollection : BusinessReadOnlyCollection<TObject, TEntry, TKey>
//        where TObject: BusinessObject<TEntry, TKey> 
//        where TEntry : class
//    {
//        //protected static object objLock = new object();

//        protected abstract TReadOnlyCollection OnCreateReadOnlyInstance(BusinessManager context, ICollection<TEntry> entries);
//        internal TReadOnlyCollection CreateReadOnlyInstance(BusinessManager context, ICollection<TEntry> entries)
//        {
//            return OnCreateReadOnlyInstance(context, entries);
//        }
//    }
//}
