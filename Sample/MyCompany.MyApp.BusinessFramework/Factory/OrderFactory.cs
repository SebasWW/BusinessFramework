using System;
using System.Collections.Generic;
using MyCompany.MyApp.Entity.Working;
using MyCompany.MyApp.EntityFramework;
using SebasWW.BusinessFramework.Factory;
using MyCompany.MyApp.EntityFramework.Entity;
using SebasWW.BusinessFramework;

namespace MyCompany.MyApp.Factory.Working
{
    public class OrderFactory : BusinessObjectFactory<Order, DTOrder, Int32>
    {
        protected override Order OnCreateInstance(BusinessManager context, DTOrder entry)
        {
            return new Order(context, entry);
        }

        static Lazy<OrderFactory> _factory = new Lazy<OrderFactory>(true);
        internal static OrderFactory Current { get => _factory.Value; }
    }

    public class OrderCollectionFactory : BusinessCollectionFactory<OrderCollection, OrderReadOnlyCollection, Order, DTOrder, Int32>
    {
        protected override OrderCollection OnCreateInstance(BusinessManager context, ICollection<DTOrder> entries)
        {
            return new OrderCollection(context, entries);
        }

        protected override OrderReadOnlyCollection OnCreateReadOnlyInstance(BusinessManager context, ICollection<DTOrder> entries)
        {
            return new OrderReadOnlyCollection(context, entries);
        }

        static Lazy<OrderCollectionFactory> _factory = new Lazy<OrderCollectionFactory>(true);
        internal static OrderCollectionFactory Current { get => _factory.Value; }
    }
}