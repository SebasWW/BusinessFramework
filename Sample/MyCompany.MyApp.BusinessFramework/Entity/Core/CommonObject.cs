using System;
using SebasWW.BusinessFramework;

namespace MyCompany.MyApp.Entity.Core
{
    abstract class CommonObject<TEntry> : MyAppObject<TEntry>
        where TEntry: class, new()
    {
        Func<TEntry, Int32> _suidSelector;

        internal CommonObject(BusinessContext BusinessContext, TEntry entry, Func<TEntry,Int32> suidSelector)
            :base(BusinessContext, entry)
        {
            _suidSelector = suidSelector;
        }

        protected CommonObject()
            : base(null, new TEntry())
        {
            //Entry.SUid = Guid.NewGuid();
        }

        public int SUid { get => _suidSelector.Invoke(Entry); }


        // *************************************
        // Methods
        // *************************************

        //protected override void OnRemove()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
