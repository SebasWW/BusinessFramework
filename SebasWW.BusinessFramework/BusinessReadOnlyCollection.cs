using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SebasWW.BusinessFramework.Factory;
using System.Collections.ObjectModel;

namespace SebasWW.BusinessFramework
{
    public abstract class BusinessReadOnlyCollection<TObject, TEntry, TKey>
        : IReadOnlyList<TObject>
        where TEntry : class
        where TObject : BusinessObject<TEntry, TKey>

    {
        object objLock = new object();

        List<TObject> _dicObj;

        BusinessContext _BusinessContext;
        ICollection<TEntry> _entrySet;
        Func<TEntry, TKey> _keySelector;
        BusinessObjectFactory<TObject, TEntry, TKey> _factory;

        protected Func<TEntry, TKey> KeySelector { get => _keySelector; }

        protected virtual ICollection<TEntry> EntrySet { get => _entrySet; }
        public BusinessContext BusinessManager
        {
            get => _BusinessContext;
            set//protected internal 
            {
                _BusinessContext = value;
                OnBusinessContextChange();
            }
        }
        virtual protected void OnBusinessContextChange()
        {
            foreach (var obj in ObjectDictionary())
            {
                obj.BusinessManager = BusinessManager;
            }
        }

        protected BusinessReadOnlyCollection(BusinessContext BusinessContext, ICollection<TEntry> entrySet, Func<TEntry, TKey> keySelector, BusinessObjectFactory<TObject, TEntry, TKey> factory)
        {
            _BusinessContext = BusinessContext;
            _entrySet = entrySet;
            _keySelector = keySelector;
            _factory = factory;
        }

        protected List<TObject> ObjectDictionary()
        {
            lock (objLock)
            {
                if (_dicObj == null)
                {
                    _dicObj = new List<TObject>();

                    foreach (var t in _entrySet)
                    {
                        _dicObj.Add(_BusinessContext.CreateBusinessObject(_factory, t));
                    }
                }
            }
            return _dicObj;
        }

        public IEnumerable<TModel> ToModelArray<TModel>(Func<TObject, TModel> elementSelector)
        {
            var c = new Collection<TModel>();

            foreach (TObject obj in Values)
            {
                //  deleted
                if ((obj as IRemovable)?.IsDeleted == true) continue;

                var m = elementSelector(obj);
                if (m != null) c.Add(m);
            }

            return c;

            //foreach (TObject obj in Values)
            //{
            //    //  deleted
            //    if ((obj as IRemovable)?.IsDeleted == true) continue;

            //    var m = elementSelector(obj);
            //    if (m != null) yield return m;
            //}
        }

        #region IReadOnlyDictionary


        public IEnumerable<TKey> Keys => ObjectDictionary().Select(t => KeySelector(t.Entry));

        public virtual IEnumerable<TObject> Values => ObjectDictionary();

        public int Count => ObjectDictionary().Count;

        public virtual bool IsReadOnly { get => true; set { } }

        public TObject this[int index] { get => _dicObj[index]; set => _dicObj[index] = value; }


        public IEnumerator<TObject> GetEnumerator()
        {
            return ObjectDictionary().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ObjectDictionary().GetEnumerator();
        }

        public int IndexOf(TObject item)
        {
            return ((IList<TObject>)_dicObj).IndexOf(item);
        }

        //public void Insert(int index, TObject item)
        //{
        //    ((IList<TObject>)_dicObj).Insert(index, item);
        //}

        //public void RemoveAt(int index)
        //{
        //    ((IList<TObject>)_dicObj).RemoveAt(index);
        //}

        //public void Add(TObject item)
        //{
        //    ((IList<TObject>)_dicObj).Add(item);
        //}

        //public void Clear()
        //{
        //    ((IList<TObject>)_dicObj).Clear();
        //}

        //public bool Contains(TObject item)
        //{
        //    return ((IList<TObject>)_dicObj).Contains(item);
        //}

        //public void CopyTo(TObject[] array, int arrayIndex)
        //{
        //    ((IList<TObject>)_dicObj).CopyTo(array, arrayIndex);
        //}

        //public bool Remove(TObject item)
        //{
        //    return ((IList<TObject>)_dicObj).Remove(item);
        //}


        #endregion

    }
}
