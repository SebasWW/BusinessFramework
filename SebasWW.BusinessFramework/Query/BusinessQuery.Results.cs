using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace SebasWW.BusinessFramework.Query
{
    public partial class BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>
    {
        //
        // Summary:
        //     Correlates the elements of two sequences based on matching keys. The default
        //     equality comparer is used to compare keys.
        //
        // Parameters:
        //   outer:
        //     The first sequence to join.
        //
        //   inner:
        //     The sequence to join to the first sequence.
        //
        //   outerKeySelector:
        //     A function to extract the join key from each element of the first sequence.
        //
        //   innerKeySelector:
        //     A function to extract the join key from each element of the second sequence.
        //
        //   resultSelector:
        //     A function to create a result element from two matching elements.
        //
        // Type parameters:
        //   TOuter:
        //     The type of the elements of the first sequence.
        //
        //   TInner:
        //     The type of the elements of the second sequence.
        //
        //   TKey:
        //     The type of the keys returned by the key selector functions.
        //
        //   TResult:
        //     The type of the result elements.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that has elements of type TResult
        //     that are obtained by performing an inner join on two sequences.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     outer or inner or outerKeySelector or innerKeySelector or resultSelector is null.
        public BusinessQueryEnumerableResult<TResult> Join<TInner, TJoinKey, TResult>(BusinessQueryEnumerableResult<TInner> inner, Expression<Func<TEntry, TJoinKey>> outerKeySelector, Expression<Func<TInner, TJoinKey>> innerKeySelector, Expression<Func<TEntry, TInner, TResult>> resultSelector)
            where TResult : class
            where TInner : class
        {
            return new BusinessQueryEnumerableResult<TResult>(FinalizeQuery().Join(inner.GetQuery(), outerKeySelector, innerKeySelector, resultSelector));
        }

        //
        // Summary:
        //     Correlates the elements of two sequences based on matching keys. A specified
        //     System.Collections.Generic.IEqualityComparer`1 is used to compare keys.
        //
        // Parameters:
        //   outer:
        //     The first sequence to join.
        //
        //   inner:
        //     The sequence to join to the first sequence.
        //
        //   outerKeySelector:
        //     A function to extract the join key from each element of the first sequence.
        //
        //   innerKeySelector:
        //     A function to extract the join key from each element of the second sequence.
        //
        //   resultSelector:
        //     A function to create a result element from two matching elements.
        //
        //   comparer:
        //     An System.Collections.Generic.IEqualityComparer`1 to hash and compare keys.
        //
        // Type parameters:
        //   TOuter:
        //     The type of the elements of the first sequence.
        //
        //   TInner:
        //     The type of the elements of the second sequence.
        //
        //   TKey:
        //     The type of the keys returned by the key selector functions.
        //
        //   TResult:
        //     The type of the result elements.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that has elements of type TResult
        //     that are obtained by performing an inner join on two sequences.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     outer or inner or outerKeySelector or innerKeySelector or resultSelector is null.
        public BusinessQueryEnumerableResult<TResult> Join<TInner, TJoinKey, TResult>(BusinessQueryEnumerableResult<TInner> inner, Expression<Func<TEntry, TJoinKey>> outerKeySelector, Expression<Func<TInner, TJoinKey>> innerKeySelector, Expression<Func<TEntry, TInner, TResult>> resultSelector, IEqualityComparer<TJoinKey> comparer)
            where TResult : class
            where TInner : class
        {
            return new BusinessQueryEnumerableResult<TResult>(FinalizeQuery().Join(inner.GetQuery(), outerKeySelector, innerKeySelector, resultSelector, comparer));
        }

        //
        // Summary:
        //     Projects each element of a sequence into a new form.
        //
        // Parameters:
        //   source:
        //     A sequence of values to invoke a transform function on.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        //   TResult:
        //     The type of the value returned by selector.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 whose elements are the result of
        //     invoking the transform function on each element of source.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public BusinessQueryEnumerableResult<TResult> Select<TResult>(Expression<Func<TEntry, TResult>> selector)
            where TResult : class
        {
            return new BusinessQueryEnumerableResult<TResult>(FinalizeQuery().Select(selector));
        }

        //
        // Summary:
        //     Projects each element of a sequence into a new form by incorporating the element's
        //     index.
        //
        // Parameters:
        //   source:
        //     A sequence of values to invoke a transform function on.
        //
        //   selector:
        //     A transform function to apply to each source element; the second parameter of
        //     the function represents the index of the source element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        //   TResult:
        //     The type of the value returned by selector.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 whose elements are the result of
        //     invoking the transform function on each element of source.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public BusinessQueryEnumerableResult<TResult> Select<TResult>(Expression<Func<TEntry, int, TResult>> selector)
            where TResult : class
        {
            return new BusinessQueryEnumerableResult<TResult>(FinalizeQuery().Select(selector));
        }

        //
        // Summary:
        //     Projects each element of a sequence to an System.Collections.Generic.IEnumerable`1,
        //     flattens the resulting sequences into one sequence, and invokes a result selector
        //     function on each element therein. The index of each source element is used in
        //     the intermediate projected form of that element.
        //
        // Parameters:
        //   source:
        //     A sequence of values to project.
        //
        //   collectionSelector:
        //     A transform function to apply to each source element; the second parameter of
        //     the function represents the index of the source element.
        //
        //   resultSelector:
        //     A transform function to apply to each element of the intermediate sequence.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        //   TCollection:
        //     The type of the intermediate elements collected by collectionSelector.
        //
        //   TResult:
        //     The type of the elements of the resulting sequence.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 whose elements are the result of
        //     invoking the one-to-many transform function collectionSelector on each element
        //     of source and then mapping each of those sequence elements and their corresponding
        //     source element to a result element.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or collectionSelector or resultSelector is null.
        public BusinessQueryEnumerableResult<TResult> SelectMany<TManyCollection, TResult>(Expression<Func<TEntry, int, IEnumerable<TManyCollection>>> collectionSelector, Expression<Func<TEntry, TManyCollection, TResult>> resultSelector)
            where TResult : class
        {
            return new BusinessQueryEnumerableResult<TResult>(FinalizeQuery().SelectMany(collectionSelector, resultSelector));
        }

        //
        // Summary:
        //     Projects each element of a sequence to an System.Collections.Generic.IEnumerable`1,
        //     flattens the resulting sequences into one sequence, and invokes a result selector
        //     function on each element therein.
        //
        // Parameters:
        //   source:
        //     A sequence of values to project.
        //
        //   collectionSelector:
        //     A transform function to apply to each element of the input sequence.
        //
        //   resultSelector:
        //     A transform function to apply to each element of the intermediate sequence.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        //   TCollection:
        //     The type of the intermediate elements collected by collectionSelector.
        //
        //   TResult:
        //     The type of the elements of the resulting sequence.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 whose elements are the result of
        //     invoking the one-to-many transform function collectionSelector on each element
        //     of source and then mapping each of those sequence elements and their corresponding
        //     source element to a result element.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or collectionSelector or resultSelector is null.
        public BusinessQueryEnumerableResult<TResult> SelectMany<TManyCollection, TResult>(Expression<Func<TEntry, IEnumerable<TManyCollection>>> collectionSelector, Expression<Func<TEntry, TManyCollection, TResult>> resultSelector)
            where TResult : class
        {
            return new BusinessQueryEnumerableResult<TResult>(FinalizeQuery().SelectMany(collectionSelector, resultSelector));
        }

        //
        // Summary:
        //     Projects each element of a sequence to an System.Collections.Generic.IEnumerable`1,
        //     and flattens the resulting sequences into one sequence. The index of each source
        //     element is used in the projected form of that element.
        //
        // Parameters:
        //   source:
        //     A sequence of values to project.
        //
        //   selector:
        //     A transform function to apply to each source element; the second parameter of
        //     the function represents the index of the source element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        //   TResult:
        //     The type of the elements of the sequence returned by selector.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 whose elements are the result of
        //     invoking the one-to-many transform function on each element of an input sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public BusinessQueryEnumerableResult<TResult> SelectMany<TResult>(Expression<Func<TEntry, int, IEnumerable<TResult>>> selector)
            where TResult : class
        {
            return new BusinessQueryEnumerableResult<TResult>(FinalizeQuery().SelectMany(selector));
        }

        //
        // Summary:
        //     Projects each element of a sequence to an System.Collections.Generic.IEnumerable`1
        //     and flattens the resulting sequences into one sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values to project.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        //   TResult:
        //     The type of the elements of the sequence returned by selector.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 whose elements are the result of
        //     invoking the one-to-many transform function on each element of the input sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public BusinessQueryEnumerableResult<TResult> SelectMany<TResult>(Expression<Func<TEntry, IEnumerable<TResult>>> selector)
            where TResult : class
        {
            return new BusinessQueryEnumerableResult<TResult>(FinalizeQuery().SelectMany(selector));
        }

        //
        // Summary:
        //     Applies a specified function to the corresponding elements of two sequences,
        //     producing a sequence of the results.
        //
        // Parameters:
        //   first:
        //     The first sequence to merge.
        //
        //   second:
        //     The second sequence to merge.
        //
        //   resultSelector:
        //     A function that specifies how to merge the elements from the two sequences.
        //
        // Type parameters:
        //   TFirst:
        //     The type of the elements of the first input sequence.
        //
        //   TSecond:
        //     The type of the elements of the second input sequence.
        //
        //   TResult:
        //     The type of the elements of the result sequence.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains merged elements of
        //     two input sequences.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     first or second is null.
        public BusinessQueryEnumerableResult<TResult> Zip<TSecond, TResult>(BusinessQueryEnumerableResult<TSecond> second, Expression<Func<TEntry, TSecond, TResult>> resultSelector)
            where TResult : class
            where TSecond : class
        {
            return new BusinessQueryEnumerableResult<TResult>(FinalizeQuery().Zip(second.GetQuery(), resultSelector));
        }
    }
}
