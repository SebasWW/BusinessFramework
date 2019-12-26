using System;
using System.Collections.Generic;
using MyCompany.MyApp.EntityFramework;
using MyCompany.MyApp.Factory.Working;
using MyCompany.MyApp.EntityFramework.Entity;
using SebasWW.BusinessFramework;

namespace MyCompany.MyApp.Entity.Working
{
    public class OrderCollection : MyAppCollection<Order, DTOrder>
    {
        internal OrderCollection(BusinessManager BusinessContext, ICollection<DTOrder> entrySet)
             : base(BusinessContext, entrySet, t => t.Id, OrderFactory.Current) { }
    }

    public class OrderReadOnlyCollection : MyAppReadOnlyCollection<Order, DTOrder>
    {
        internal OrderReadOnlyCollection(BusinessManager BusinessContext, ICollection<DTOrder> entrySet)
             : base(BusinessContext, entrySet, t => t.Id, OrderFactory.Current) { }
    }
}