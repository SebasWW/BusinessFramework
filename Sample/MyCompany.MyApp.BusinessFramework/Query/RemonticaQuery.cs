using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SebasWW.BusinessFramework;
using SebasWW.BusinessFramework.Query;


namespace MyCompany.MyApp.Query
{
    public class MyAppQuery<TCollection, TReadOnlyCollection, TObject, TEntry> : BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, Int32>
        where TCollection : BusinessCollection<TObject, TEntry, Int32>
        where TReadOnlyCollection : BusinessReadOnlyCollection<TObject, TEntry, Int32>
        where TObject : BusinessObject<TEntry, Int32>
        where TEntry : class
    {
        public MyAppQuery(
            BusinessQueryContext<TCollection, TReadOnlyCollection, TObject, TEntry, Int32> businessQueryContext,
            IQueryable<TEntry> query
            ) : base(businessQueryContext, query) { }
    }
}
