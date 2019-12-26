using System;
using System.Collections.Generic;
using System.Linq;
using SebasWW.BusinessFramework.Factory;

namespace SebasWW.BusinessFramework
{
    public abstract class BusinessCollection<TObject, TEntry, TKey> 
        : BusinessReadOnlyCollection<TObject, TEntry, TKey>, ICollection<TObject>
        where TEntry : class
        where TObject: BusinessObject<TEntry,TKey>
    {
        protected BusinessCollection(
            BusinessManager businessContext, 
            ICollection<TEntry> entrySet, 
            Func<TEntry, TKey> keySelector, 
            BusinessObjectFactory<TObject, TEntry, TKey> factory
            )
            :base(businessContext, entrySet, keySelector, factory) {}

        public static void Add(BusinessManager businessContext, TObject obj)
        {
            businessContext.AddBusinessObject(obj, true);
        }

        public override bool IsReadOnly { get => false; }

        public void Merge<TModel>( 
            IEnumerable<TModel> models, 
            Func<TModel, TObject, Boolean> comparer,
            Func<TModel, TObject> insertTransform, 
            Action<TModel,TObject> updateTransform,
            Func<TModel, Boolean> isEntryNew
            )
        {
            //удаляем
            foreach (TObject obj in this.Where(t => KeySelector(t.Entry) != null).ToArray())
            {
                if (!models.Where(m => comparer(m, obj)).Any())
                {
                    Remove(obj);
                }
            }

            //добавляем
            foreach (TModel model in models)
            {	
                TObject obj = this.Where(o => ((o as IRemovable)?.IsDeleted == false) && comparer(model, o)).FirstOrDefault();

                if (isEntryNew(model) || obj == null)
                {
                    Add(insertTransform(model));
                }
                else
                    updateTransform(model, obj);
            }
        }

        public void Merge<TModel>(
            IEnumerable<TModel> models,
            Func<TModel, TObject, Boolean> comparer,
            Action<TModel, TObject> updateTransform
            )
        {
            //добавляем
            foreach (TModel model in models)
            {
                TObject obj = this.Where(o => ((o as IRemovable).IsDeleted == false) && comparer(model, o)).FirstOrDefault();

                if (obj != null)
                    updateTransform(model, obj);
            }
        }

        #region ICollection 
        public virtual void Add(TObject obj)
        {
            BusinessManager?.AddBusinessObject(obj, false);

            ObjectDictionary().Add(obj);
            EntrySet.Add(obj.Entry);
        }

        public virtual void Clear()
        {
            //удаляем
            //Queue<TObject> queue = new Queue<TObject>();
            foreach (TObject obj in Values)
            {
                obj.OnRemove();
            }

            ObjectDictionary().Clear();
        }

        public bool Contains(TObject item)
        {
            return ObjectDictionary().Contains(item);
        }

        public void CopyTo(TObject[] array, int arrayIndex)
        {
            ObjectDictionary().ToArray().CopyTo(array, arrayIndex);
        }

        public virtual bool Remove(TObject item)
        {
            item.OnRemove();
            return ObjectDictionary().Remove(item);
        }

        IEnumerator<TObject> IEnumerable<TObject>.GetEnumerator()
        {
            return ObjectDictionary().GetEnumerator();
        }

#endregion
    }
}
