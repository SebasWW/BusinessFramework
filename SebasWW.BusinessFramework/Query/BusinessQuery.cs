using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SebasWW.BusinessFramework.Query
{
    public partial class BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> : 
        BusinessQueryEnumerableResult<TEntry>
        where TCollection : BusinessCollection<TObject, TEntry, TKey>
        where TReadOnlyCollection : BusinessReadOnlyCollection<TObject, TEntry, TKey>
        where TObject : BusinessObject<TEntry, TKey> 
        where TEntry : class
    {
        internal readonly BusinessQueryContext<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> BusinessQueryContext;

        public BusinessQuery(
            BusinessQueryContext<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> businessQueryContext,
            IQueryable<TEntry> query
        ) : base(query)
        {
            BusinessQueryContext = businessQueryContext;
        }

        public async Task LoadAsync(CancellationToken cancellationToken = default)
		{
            await FinalizeQuery().LoadAsync(cancellationToken);
		}
         
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> ExecCustomFunction(object customFuncKey)
        {
            if (BusinessQueryContext.CustomFilters == null) throw new InvalidOperationException("Custom functions is not initialized.");

            Query = BusinessQueryContext.CustomFilters[customFuncKey].ApplyFilter(Query, BusinessQueryContext);
            return this;
        }

        private IQueryable<TEntry> FinalizeQuery()
        {
            return BusinessQueryContext.FinalizingFilter.ApplyFilter(Query, BusinessQueryContext);
        }

		internal override IQueryable<TEntry> GetQuery()
        {
            return FinalizeQuery();
        }

		#region OrderBy
		//
		// Summary:
		//     Sorts the elements of a sequence in ascending order by using a specified comparer.
		//
		// Parameters:
		//   source:
		//     A sequence of values to order.
		//
		//   keySelector:
		//     A function to extract a key from an element.
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
		public BusinessOrderedQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> OrderBy<TSortKey>(Expression<Func<TEntry, TSortKey>> keySelector, IComparer<TSortKey> comparer)
        {
            return new BusinessOrderedQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>(this, Query.OrderBy(keySelector, comparer));
        }

        //
        // Summary:
        //     Sorts the elements of a sequence in ascending order according to a key.
        //
        // Parameters:
        //   source:
        //     A sequence of values to order.
        //
        //   keySelector:
        //     A function to extract a key from an element.
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
        public BusinessOrderedQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> OrderBy<TSortKey>(Expression<Func<TEntry, TSortKey>> keySelector)
        {
            return new BusinessOrderedQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>(this, Query.OrderBy(keySelector));
        }

        //
        // Summary:
        //     Sorts the elements of a sequence in descending order according to a key.
        //
        // Parameters:
        //   source:
        //     A sequence of values to order.
        //
        //   keySelector:
        //     A function to extract a key from an element.
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
        public BusinessOrderedQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> OrderByDescending<TSortKey>(Expression<Func<TEntry, TSortKey>> keySelector)
        {
            return new BusinessOrderedQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>(this, Query.OrderByDescending(keySelector));
        }

        //
        // Summary:
        //     Sorts the elements of a sequence in descending order by using a specified comparer.
        //
        // Parameters:
        //   source:
        //     A sequence of values to order.
        //
        //   keySelector:
        //     A function to extract a key from an element.
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
        public BusinessOrderedQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> OrderByDescending<TSortKey>(Expression<Func<TEntry, TSortKey>> keySelector, IComparer<TSortKey> comparer)
        {
            return new BusinessOrderedQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>(this, Query.OrderByDescending(keySelector, comparer));
        }
        #endregion

        #region Include

        ////
        //// Summary:
        ////     Specifies related entities to include in the query results. The navigation property
        ////     to be included is specified starting with the type of entity being queried (TEntity).
        ////     If you wish to include additional types based on the navigation properties of
        ////     the type being included, then chain a call to Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ThenInclude``3(Microsoft.EntityFrameworkCore.Query.IIncludableQueryable{``0,System.Collections.Generic.IEnumerable{``1}},System.Linq.Expressions.Expression{System.Func{``1,``2}})
        ////     after this call.
        ////
        //// Parameters:
        ////   source:
        ////     The source query.
        ////
        ////   navigationPropertyPath:
        ////     A lambda expression representing the navigation property to be included (t =>
        ////     t.Property1).
        ////
        //// Type parameters:
        ////   TEntity:
        ////     The type of entity being queried.
        ////
        ////   TProperty:
        ////     The type of the related entity to be included.
        ////
        //// Returns:
        ////     A new query with the related data included.
        //public BusinessIncludableQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey, TProperty> 
        //    Include<TProperty>(Expression<Func<TEntry, TProperty>> navigationPropertyPath) where TProperty:class
        //{
        //    return new BusinessIncludableQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey, TProperty>
        //        (
        //            this, 
        //            Query.Include<TEntry,TProperty>(navigationPropertyPath) 
        //         );
        //}

        //public BusinessIncludableCollectionQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey, TProperty>
        //    Include<TProperty>(Expression<Func<TEntry, ICollection<TProperty>>> navigationPropertyPath) where TProperty : class
        //{
        //    return new BusinessIncludableCollectionQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey, TProperty>
        //        (
        //            this,
        //            Query.Include<TEntry, ICollection<TProperty>>(navigationPropertyPath)
        //         );
        //}
#endregion

        #region ToObject
        //
        // Summary:
        //     Applies an accumulator function over a sequence.
        //
        // Parameters:
        //   source:
        //     A sequence to aggregate over.
        //
        //   func:
        //     An accumulator function to apply to each element.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The final accumulator value.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or func is null.
        //
        //   T:System.InvalidOperationException:
        //     source contains no elements.
        public TObject Aggregate(Expression<Func<TEntry, TEntry, TEntry>> func)
        {

            return BusinessQueryContext.BusinessContext.CreateBusinessObject(BusinessQueryContext.ObjectFactory, FinalizeQuery().Aggregate(func));
        }

        //
        // Summary:
        //     Returns the element at a specified index in a sequence.
        //
        // Parameters:
        //   source:
        //     An System.Linq.IQueryable`1 to return an element from.
        //
        //   index:
        //     The zero-based index of the element to retrieve.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The element at the specified position in source.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     index is less than zero.
        public TObject ElementAt( int index)
        {
            return BusinessQueryContext.BusinessContext.CreateBusinessObject(BusinessQueryContext.ObjectFactory, FinalizeQuery().ElementAt(index));
        }

        //
        // Summary:
        //     Returns the element at a specified index in a sequence or a default value if
        //     the index is out of range.
        //
        // Parameters:
        //   source:
        //     An System.Linq.IQueryable`1 to return an element from.
        //
        //   index:
        //     The zero-based index of the element to retrieve.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     default(TSource) if index is outside the bounds of source; otherwise, the element
        //     at the specified position in source.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.

        public TObject ElementAtOrDefault( int index)
        {
            return BusinessQueryContext.BusinessContext.CreateBusinessObject(BusinessQueryContext.ObjectFactory, FinalizeQuery().ElementAtOrDefault(index));
        }

        //
        // Summary:
        //     Returns the first element of a sequence.
        //
        // Parameters:
        //   source:
        //     The System.Linq.IQueryable`1 to return the first element of.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The first element in source.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        //
        //   T:System.InvalidOperationException:
        //     The source sequence is empty.
        public async Task<TObject> FirstAsync()
        {
            return BusinessQueryContext.BusinessContext.CreateBusinessObject(BusinessQueryContext.ObjectFactory, await FinalizeQuery().FirstAsync());
        }

        //
        // Summary:
        //     Returns the first element of a sequence.
        //
        // Parameters:
        //   source:
        //     The System.Linq.IQueryable`1 to return the first element of.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The first element in source.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        //
        //   T:System.InvalidOperationException:
        //     The source sequence is empty.
        public TObject First()
        {
            return BusinessQueryContext.BusinessContext.CreateBusinessObject(BusinessQueryContext.ObjectFactory, FinalizeQuery().First());
        }

        //
        // Summary:
        //     Returns the first element of a sequence that satisfies a specified condition.
        //
        // Parameters:
        //   source:
        //     An System.Linq.IQueryable`1 to return an element from.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The first element in source that passes the test in predicate.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        //
        //   T:System.InvalidOperationException:
        //     No element satisfies the condition in predicate. -or- The source sequence is
        //     empty.
        public async Task<TObject> FirstAsync( Expression<Func<TEntry, bool>> predicate)
        {
            return BusinessQueryContext.BusinessContext.CreateBusinessObject<TObject, TEntry, TKey>(BusinessQueryContext.ObjectFactory, await FinalizeQuery().FirstAsync(predicate));
        }

        //
        // Summary:
        //     Returns the first element of a sequence, or a default value if the sequence contains
        //     no elements.
        //
        // Parameters:
        //   source:
        //     The System.Linq.IQueryable`1 to return the first element of.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     default(TSource) if source is empty; otherwise, the first element in source.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public async Task<TObject> FirstOrDefaultAsync()
        {
            return BusinessQueryContext.BusinessContext.CreateBusinessObject(BusinessQueryContext.ObjectFactory, await FinalizeQuery().FirstOrDefaultAsync());
        }
        //
        // Summary:
        //     Returns the first element of a sequence, or a default value if the sequence contains
        //     no elements.
        //
        // Parameters:
        //   source:
        //     The System.Linq.IQueryable`1 to return the first element of.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     default(TSource) if source is empty; otherwise, the first element in source.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public TObject FirstOrDefault()
        {
            var result = FinalizeQuery().FirstOrDefault();
            if (result == null)
                return null;
            else 
                return BusinessQueryContext.BusinessContext.CreateBusinessObject(BusinessQueryContext.ObjectFactory, result);
        }

        //
        // Summary:
        //     Returns the first element of a sequence that satisfies a specified condition
        //     or a default value if no such element is found.
        //
        // Parameters:
        //   source:
        //     An System.Linq.IQueryable`1 to return an element from.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     default(TSource) if source is empty or if no element passes the test specified
        //     by predicate; otherwise, the first element in source that passes the test specified
        //     by predicate.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        public async Task<TObject> FirstOrDefaultAsync(Expression<Func<TEntry, bool>> predicate)
        {
            var result = await FinalizeQuery().FirstOrDefaultAsync(predicate);
            if (result == null)
                return null;
            else
                return BusinessQueryContext.BusinessContext.CreateBusinessObject(BusinessQueryContext.ObjectFactory, result);
        }

        //
        // Summary:
        //     Returns the last element of a sequence that satisfies a specified condition.
        //
        // Parameters:
        //   source:
        //     An System.Linq.IQueryable`1 to return an element from.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The last element in source that passes the test specified by predicate.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        //
        //   T:System.InvalidOperationException:
        //     No element satisfies the condition in predicate. -or- The source sequence is
        //     empty.
        public async Task<TObject> LastAsync(Expression<Func<TEntry, bool>> predicate)
        {
            return BusinessQueryContext.BusinessContext.CreateBusinessObject(BusinessQueryContext.ObjectFactory, await FinalizeQuery().LastAsync(predicate));
        }
        
        //
        // Summary:
        //     Returns the last element in a sequence.
        //
        // Parameters:
        //   source:
        //     An System.Linq.IQueryable`1 to return the last element of.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The value at the last position in source.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        //
        //   T:System.InvalidOperationException:
        //     The source sequence is empty.
        public async Task<TObject> LastAsync()
        {
            return BusinessQueryContext.BusinessContext.CreateBusinessObject(BusinessQueryContext.ObjectFactory,await FinalizeQuery().LastAsync());
        }
        
        //
        // Summary:
        //     Returns the last element of a sequence that satisfies a condition or a default
        //     value if no such element is found.
        //
        // Parameters:
        //   source:
        //     An System.Linq.IQueryable`1 to return an element from.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     default(TSource) if source is empty or if no elements pass the test in the predicate
        //     function; otherwise, the last element of source that passes the test in the predicate
        //     function.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        public async Task<TObject> LastOrDefaultAsync(Expression<Func<TEntry, bool>> predicate)
        {
            var result = await FinalizeQuery().LastOrDefaultAsync(predicate);
            if (result == null)
                return null;
            else
                return BusinessQueryContext.BusinessContext.CreateBusinessObject(BusinessQueryContext.ObjectFactory, result);
        }
        
        //
        // Summary:
        //     Returns the last element in a sequence, or a default value if the sequence contains
        //     no elements.
        //
        // Parameters:
        //   source:
        //     An System.Linq.IQueryable`1 to return the last element of.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     default(TSource) if source is empty; otherwise, the last element in source.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public async Task<TObject> LastOrDefaultAsync()
        {
            var result = await FinalizeQuery().LastOrDefaultAsync();
            if (result == null)
                return null;
            else
                return BusinessQueryContext.BusinessContext.CreateBusinessObject(BusinessQueryContext.ObjectFactory, result);
        }

        //
        // Summary:
        //     Returns the maximum value in a generic System.Linq.IQueryable`1.
        //
        // Parameters:
        //   source:
        //     A sequence of values to determine the maximum of.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The maximum value in the sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public async Task<TObject> MaxAsync()
        {
            return BusinessQueryContext.BusinessContext.CreateBusinessObject(BusinessQueryContext.ObjectFactory, await FinalizeQuery().MaxAsync());
        }

        //
        // Summary:
        //     Returns the minimum value of a generic System.Linq.IQueryable`1.
        //
        // Parameters:
        //   source:
        //     A sequence of values to determine the minimum of.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The minimum value in the sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public async Task<TObject> MinAsync()
        {
            return BusinessQueryContext.BusinessContext.CreateBusinessObject(BusinessQueryContext.ObjectFactory, await FinalizeQuery().MinAsync());
        }

        //
        // Summary:
        //     Returns the only element of a sequence, and throws an exception if there is not
        //     exactly one element in the sequence.
        //
        // Parameters:
        //   source:
        //     An System.Linq.IQueryable`1 to return the single element of.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The single element of the input sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        //
        //   T:System.InvalidOperationException:
        //     source has more than one element.
        public async Task<TObject> SingleAsync()
        {
            return BusinessQueryContext.BusinessContext.CreateBusinessObject(BusinessQueryContext.ObjectFactory, await FinalizeQuery().SingleAsync());
        }

        //
        // Summary:
        //     Returns the only element of a sequence that satisfies a specified condition,
        //     and throws an exception if more than one such element exists.
        //
        // Parameters:
        //   source:
        //     An System.Linq.IQueryable`1 to return a single element from.
        //
        //   predicate:
        //     A function to test an element for a condition.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The single element of the input sequence that satisfies the condition in predicate.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        //
        //   T:System.InvalidOperationException:
        //     No element satisfies the condition in predicate. -or- More than one element satisfies
        //     the condition in predicate. -or- The source sequence is empty.
        public async Task<TObject> SingleAsync(Expression<Func<TEntry, bool>> predicate)
        {
            return BusinessQueryContext.BusinessContext.CreateBusinessObject(BusinessQueryContext.ObjectFactory, await FinalizeQuery().SingleAsync(predicate));
        }

        //
        // Summary:
        //     Returns the only element of a sequence, or a default value if the sequence is
        //     empty; this method throws an exception if there is more than one element in the
        //     sequence.
        //
        // Parameters:
        //   source:
        //     An System.Linq.IQueryable`1 to return the single element of.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The single element of the input sequence, or default(TSource) if the sequence
        //     contains no elements.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        //
        //   T:System.InvalidOperationException:
        //     source has more than one element.
        public async Task<TObject> SingleOrDefaultAsync()
        {
            var result = await FinalizeQuery().SingleOrDefaultAsync();
            if (result == null)
                return null;
            else
                return BusinessQueryContext.BusinessContext.CreateBusinessObject(BusinessQueryContext.ObjectFactory, result);
        }

        //
        // Summary:
        //     Returns the only element of a sequence that satisfies a specified condition or
        //     a default value if no such element exists; this method throws an exception if
        //     more than one element satisfies the condition.
        //
        // Parameters:
        //   source:
        //     An System.Linq.IQueryable`1 to return a single element from.
        //
        //   predicate:
        //     A function to test an element for a condition.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The single element of the input sequence that satisfies the condition in predicate,
        //     or default(TSource) if no such element is found.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        //
        //   T:System.InvalidOperationException:
        //     More than one element satisfies the condition in predicate.
        public async Task<TObject> SingleOrDefaultAsync(Expression<Func<TEntry, bool>> predicate)
        {
            var result = await FinalizeQuery().SingleOrDefaultAsync(predicate);
            if (result == null)
                return null;
            else
                return BusinessQueryContext.BusinessContext.CreateBusinessObject(BusinessQueryContext.ObjectFactory, result);
        }
		#endregion

		#region ToEnum

		//public async Task<IEnumerable<TObject>> ToArray()
		//{
		//    return CreateObjectArray(Query.ToArray(), e => BusinessQueryContext.BusinessContext.CreateBusinessObject(BusinessQueryContext.ObjectFactory, e));
		//}

		//
		// Summary:
		//     Creates an business collection 
		//
		// Parameters:
		//
		// Type parameters:
		//   TSource:
		//     The type of the elements of source.
		//
		// Returns:
		//     An array that contains the elements from the input sequence.
		//
		// Exceptions:
		//   T:System.ArgumentNullException:
		//     source is null.
		//public TCollection ToCollection()
  //      {
  //          return BusinessQueryContext.CollectionFactory.CreateInstance(BusinessQueryContext.BusinessContext, SecureReadQuery().ToArray());
  //      }

        public async Task<TCollection> ToCollectionAsync()
        {
            return BusinessQueryContext.CollectionFactory.CreateInstance(BusinessQueryContext.BusinessContext, await FinalizeQuery().ToArrayAsync());
        }
        //public TReadOnlyCollection ToReadOnlyCollection()
        //{
        //    return BusinessQueryContext.CollectionFactory.CreateReadOnlyInstance(BusinessQueryContext.BusinessContext, SecureReadQuery().AsNoTracking().ToArray());
        //}

        //public async Task<TReadOnlyCollection> ToReadOnlyCollectionAsync()
        //{
        //    return  BusinessQueryContext.CollectionFactory.CreateReadOnlyInstance(BusinessQueryContext.BusinessContext, await SecureReadQuery().AsNoTracking().ToArrayAsync());
        //}

		//private IEnumerable<TObject> CreateObjectArray(IEnumerable<TEntry> entrySet, Func<TEntry, TObject> elementSelector)
		//{
		//    foreach (var obj in entrySet)
		//    {
		//        yield return elementSelector(obj);
		//    }
		//}
		#endregion
	}
}
