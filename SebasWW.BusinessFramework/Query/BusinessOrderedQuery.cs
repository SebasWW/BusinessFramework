using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SebasWW.BusinessFramework.Query;

namespace SebasWW.BusinessFramework.Query
{
    public class BusinessOrderedQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> : BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>
        where TCollection : GenericCollection<TObject, TEntry, TKey>
        where TReadOnlyCollection : GenericReadOnlyCollection<TObject, TEntry, TKey>
        where TObject : GenericObject<TEntry, TKey>
        where TEntry : class
    {
        IOrderedQueryable<TEntry> _orderedQueryable;

        public BusinessOrderedQuery(
            BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> businessQuery,
            IOrderedQueryable<TEntry> query
            ) :base(businessQuery.BusinessQueryContext,query)
        {
            _orderedQueryable = query;
        }

        //
        // Summary:
        //     Performs a subsequent ordering of the elements in a sequence in ascending order
        //     according to a key.
        //
        // Parameters:
        //
        //   keySelector:
        //     A function to extract a key from each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        // Returns:
        //     An System.Linq.IOrderedEnumerable`1 whose elements are sorted according to a
        //     key.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or keySelector is null.
        public BusinessOrderedQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> ThenBy<TSortKey>(Expression<Func<TEntry, TSortKey>> keySelector)
        {
            return new BusinessOrderedQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>(this, _orderedQueryable.ThenBy(keySelector));
        }

        //
        // Summary:
        //     Performs a subsequent ordering of the elements in a sequence in ascending order
        //     by using a specified comparer.
        //
        // Parameters:
        //
        //   keySelector:
        //     A function to extract a key from each element.
        //
        //   comparer:
        //     An System.Collections.Generic.IComparer`1 to compare keys.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        // Returns:
        //     An System.Linq.IOrderedEnumerable`1 whose elements are sorted according to a
        //     key.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or keySelector is null.
        public BusinessOrderedQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> ThenBy<TSortKey>(Expression<Func<TEntry, TSortKey>> keySelector, IComparer<TSortKey> comparer)
        {
            return new BusinessOrderedQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>(this, _orderedQueryable.ThenBy(keySelector, comparer));
        }

        //
        // Summary:
        //     Performs a subsequent ordering of the elements in a sequence in descending order
        //     by using a specified comparer.
        //
        // Parameters:
        //   source:
        //     An System.Linq.IOrderedEnumerable`1 that contains elements to sort.
        //
        //   keySelector:
        //     A function to extract a key from each element.
        //
        //   comparer:
        //     An System.Collections.Generic.IComparer`1 to compare keys.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        // Returns:
        //     An System.Linq.IOrderedEnumerable`1 whose elements are sorted in descending order
        //     according to a key.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or keySelector is null.
        public BusinessOrderedQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> ThenByDescending<TSortKey>(Expression<Func<TEntry, TSortKey>> keySelector, IComparer<TSortKey> comparer)
        {
            return new BusinessOrderedQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>(this, _orderedQueryable.ThenByDescending(keySelector, comparer));
        }

        //`
        // Summary:
        //     Performs a subsequent ordering of the elements in a sequence in descending order,
        //     according to a key.
        //
        // Parameters:
        //   source:
        //     An System.Linq.IOrderedEnumerable`1 that contains elements to sort.
        //
        //   keySelector:
        //     A function to extract a key from each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        // Returns:
        //     An System.Linq.IOrderedEnumerable`1 whose elements are sorted in descending order
        //     according to a key.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or keySelector is null.
        public BusinessOrderedQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> ThenByDescending<TSortKey>(Expression<Func<TEntry, TSortKey>> keySelector)
        {
            return new BusinessOrderedQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>(this, _orderedQueryable.ThenByDescending(keySelector));
        }
    }
}
