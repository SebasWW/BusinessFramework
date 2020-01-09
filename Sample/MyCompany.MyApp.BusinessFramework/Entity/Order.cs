using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyCompany.MyApp.EntityFramework;
using MyCompany.MyApp.EntityFramework.Entity;
using SebasWW.BusinessFramework;

namespace MyCompany.MyApp.Entity
{
    public sealed class Order :
        MyAppObject<DTOrder>,
        IRemovable, IConsistencyValidator
    {
        internal Order(BusinessContext businessManager, DTOrder entry)
            : base(businessManager, entry) { }

        internal Order(DTOrder entry)
            : base(entry) { }

        public Order() : base(new DTOrder()) { }

        // ************************************
        // Свойства	
        // *************************************
        public int Id { get => Entry.Id; }
        public bool IsDeleted { get => ((BusinessManager as MyAppBusinessContext).DbContext.ChangeTracker.Entries<DTOrder>().FirstOrDefault(t => t.Entity.Id == Id)?.State ?? EntityState.Detached) == EntityState.Deleted; }
        
        public string Name { get => Entry.Name; set => Entry.Name = value; }

        public int Company1Id { get => Entry.Company1Id; set => Entry.Company1Id = value; }
        public int Company2Id { get => Entry.Company2Id; set => Entry.Company2Id = value; }
        public int UserId { get => Entry.UserId; set => Entry.UserId = value; }

        // *************************************	
        // Родителькие связанные объекты
        // *************************************
        //// t_order
        //ServicePackage _servicePackage;
        //public ServicePackage ServicePackage
        //{
        //    get
        //    {
        //        if (Entry.ServicePackage == null) return null;

        //        lock (objLock)
        //        {
        //            if (_servicePackage == null)
        //                _servicePackage =
        //                    (BusinessManager as MyAppManager).CreateObject(ServicePackageFactory.Current, Entry.ServicePackage);
        //        }

        //        return _servicePackage;
        //    }
        //}
        // *************************************	
        // Дочерние связанные коллекции	
        // *************************************	

        //// t_order_room
        //OrderRoomCollection _orderRooms;
        //public OrderRoomCollection OrderRooms
        //{
        //    get
        //    {
        //        lock (objLock)
        //        {
        //            if (_orderRooms == null)
        //                _orderRooms =
        //                    new OrderRoomCollection(BusinessManager, Entry.TOrderRoom);
        //        }
        //        return _orderRooms;
        //    }
        //}



        // *************************************	
        // Checks
        // ************************************
        IQueryable<ConsistencyErrorEntry> IConsistencyValidator.CheckAsQuery(bool beforeUpdate)
        {
            IQueryable<ConsistencyErrorEntry> q, query = null;

            // shared checks ....

            // pre/post update checks
            if (beforeUpdate)
            {
                // ...
            }
            else
            {
                //if (Entry.SArchive == 1)
                //{
                //    q = ((BusinessManager as MyAppManager).DbContext as MyAppContext).TOrderRoom
                //        .Where(t => t.OrderId == Id && t.SArchive == 0)
                //        .Select(t => new ConsistencyErrorEntry(Id, "ERROR:T_ORDER:DELETE:T_ORDER_ROOM_IS_EXISTS"
                //            , "Существуют неудалённые T_ORDER_ROOM."));

                //    query = query?.Union(q) ?? q;
                //}
            }

            return query;
        }
        // ************************************
        // Set context
        // ************************************
        protected override void OnBusinessManagerChange()
        {
            //OrderRooms.BusinessManager = BusinessManager;
        }

        // *************************************	
        // Methods
        // ************************************
        //public void Remove()
        //{
        //    OnRemove();
        //}

        protected override void OnRemove()
        {
            // OrderRooms.Clear();

            ((BusinessManager as MyAppBusinessContext).DbContext as MyAppDbContext).Orders.Remove(Entry); //Entry.SArchive = 1;
        }
    }
}
