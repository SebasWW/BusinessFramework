using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SebasWW.BusinessFramework
{
    public abstract class GenericObject<TEntry, TKey>: GenericObjectBase
        where TEntry:class 
    {
        protected Object objLock = new Object();

        #region Changing tracking

        //public delegate void OnChangingEventHandler(object sender, OnChangingEventArgs<GenericObject<TEntry, TKey>> e);
        //static public event OnChangingEventHandler Changing;

        //internal override void OnChanging()
        //{
        //    Changing?.Invoke(this, new OnChangingEventArgs<GenericObject<TEntry, TKey>>( this));
        //}

        //internal override void OnChangedUncommitted()
        //{
        //    // Changing?.Invoke(this, new OnChangingEventArgs<TEntry>( Entry));
        //}

        //public delegate void OnChangedEventHandler(object sender, OnChangedEventArgs<GenericObject<TEntry, TKey>> e);
        //static public event OnChangedEventHandler Changed;

        //internal override void OnChanged()
        //{
        //    Changed?.Invoke(this, new OnChangedEventArgs<GenericObject<TEntry, TKey>>(this));
        //}

        #endregion

        public readonly Guid Guid = Guid.NewGuid();
        protected internal TEntry Entry { get; }

        BusinessManager _BusinessManager;
        protected internal BusinessManager BusinessManager {
            get => _BusinessManager;
            set
            {
                _BusinessManager = value;
                _BusinessManager.RegisterBusinessObject(this);

                OnBusinessManagerChange();
            }
        }

        protected GenericObject(BusinessManager BusinessManager, TEntry entry)
        {
            Entry = entry ?? throw new Exception("Creating business object without entry.");
            _BusinessManager = BusinessManager;
        }

        protected GenericObject(TEntry entry)
        {
            Entry = entry ?? throw new Exception("Creating business object without entry.");
        }

        protected internal abstract void OnRemove();

        virtual protected void OnBusinessManagerChange() { }
        
        // secure query
        protected virtual internal IQueryable<TEntry> PermissionRead(IQueryable<TEntry> query) { return query;}
    }
}
