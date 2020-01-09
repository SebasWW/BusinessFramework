using System.Collections.Generic;
using MyCompany.MyApp.EntityFramework.Entity;
using MyCompany.MyApp.Factory;
using SebasWW.BusinessFramework;

namespace MyCompany.MyApp.Entity
{
    public class OrderCollection : MyAppCollection<Order, DTOrder>
    {
        internal OrderCollection(BusinessContext BusinessContext, ICollection<DTOrder> entrySet)
             : base(BusinessContext, entrySet, t => t.Id, OrderFactory.Current) { }
    }

    public class OrderReadOnlyCollection : MyAppReadOnlyCollection<Order, DTOrder>
    {
        internal OrderReadOnlyCollection(BusinessContext BusinessContext, ICollection<DTOrder> entrySet)
             : base(BusinessContext, entrySet, t => t.Id, OrderFactory.Current) { }
    }
}