using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SebasWW.BusinessFramework.Query;

namespace SebasWW.BusinessFramework
{
    public partial class BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>
    {

        ////
        //// Summary:
        ////     Applies an accumulator function over a sequence. The specified seed value is
        ////     used as the initial accumulator value, and the specified function is used to
        ////     select the result value.
        ////
        //// Parameters:
        ////   source:
        ////     A sequence to aggregate over.
        ////
        ////   seed:
        ////     The initial accumulator value.
        ////
        ////   func:
        ////     An accumulator function to invoke on each element.
        ////
        ////   selector:
        ////     A function to transform the final accumulator value into the result value.
        ////
        //// Type parameters:
        ////   TSource:
        ////     The type of the elements of source.
        ////
        ////   TAccumulate:
        ////     The type of the accumulator value.
        ////
        ////   TResult:
        ////     The type of the resulting value.
        ////
        //// Returns:
        ////     The transformed final accumulator value.
        ////
        //// Exceptions:
        ////   T:System.ArgumentNullException:
        ////     source or func or selector is null.
        //public BusinessQuery<TResult> Aggregate<TAccumulate, TResult>(TAccumulate seed, Expression<Func<TAccumulate, TEntry, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector)
        //{
        //    return new BusinessQuery<TResult>( query.Aggregate(seed, func, selector));
        //}

        //
        // Parameters:

        //   element:
        //
        // Type parameters:
        //   TSource:
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> Append(TEntry element)
        {
            Query = Query.Append(element);
            return this;
        }

        //
        // Summary:
        //     Concatenates two sequences.
        //
        // Parameters:
        //   first:
        //     The first sequence to concatenate.
        //
        //   second:
        //     The sequence to concatenate to the first sequence.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of the input sequences.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains the concatenated elements
        //     of the two input sequences.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     first or second is null.
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> Concat(BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> second)
        {
            Query = Query.Concat(second.Query);
            return this;
        }

        //
        // Summary:
        //     Returns the elements of the specified sequence or the specified value in a singleton
        //     collection if the sequence is empty.
        //
        // Parameters:
        //   source:
        //     The sequence to return the specified value for if it is empty.
        //
        //   defaultValue:
        //     The value to return if the sequence is empty.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains defaultValue if source
        //     is empty; otherwise, source.
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> DefaultIfEmpty(TEntry defaultValue)
        {
            Query = Query.DefaultIfEmpty(defaultValue);
            return this;
        }

        //
        // Summary:
        //     Returns the elements of the specified sequence or the type parameter's default
        //     value in a singleton collection if the sequence is empty.
        //
        // Parameters:
        //   source:
        //     The sequence to return a default value for if it is empty.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 object that contains the default
        //     value for the TSource type if source is empty; otherwise, source.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> DefaultIfEmpty()
        {
            Query = Query.DefaultIfEmpty();
            return this;
        }

        //
        // Summary:
        //     Returns distinct elements from a sequence by using the default equality comparer
        //     to compare values.
        //
        // Parameters:
        //   source:
        //     The sequence to remove duplicate elements from.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains distinct elements from
        //     the source sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> Distinct()
        {
            Query = Query.Distinct();
            return this;
        }

        //
        // Summary:
        //     Returns distinct elements from a sequence by using a specified System.Collections.Generic.IEqualityComparer`1
        //     to compare values.
        //
        // Parameters:
        //   source:
        //     The sequence to remove duplicate elements from.
        //
        //   comparer:
        //     An System.Collections.Generic.IEqualityComparer`1 to compare values.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains distinct elements from
        //     the source sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> Distinct(IEqualityComparer<TEntry> comparer)
        {
            Query = Query.Distinct(comparer);
            return this;
        }

        //
        // Summary:
        //     Produces the set difference of two sequences by using the default equality comparer
        //     to compare values.
        //
        // Parameters:
        //   first:
        //     An System.Collections.Generic.IEnumerable`1 whose elements that are not also
        //     in second will be returned.
        //
        //   second:
        //     An System.Collections.Generic.IEnumerable`1 whose elements that also occur in
        //     the first sequence will cause those elements to be removed from the returned
        //     sequence.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of the input sequences.
        //
        // Returns:
        //     A sequence that contains the set difference of the elements of two sequences.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     first or second is null.
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> Except(BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> second)
        {
            Query = Query.Except(second.Query);
            return this;
        }
        //
        // Summary:
        //     Produces the set difference of two sequences by using the specified System.Collections.Generic.IEqualityComparer`1
        //     to compare values.
        //
        // Parameters:
        //   first:
        //     An System.Collections.Generic.IEnumerable`1 whose elements that are not also
        //     in second will be returned.
        //
        //   second:
        //     An System.Collections.Generic.IEnumerable`1 whose elements that also occur in
        //     the first sequence will cause those elements to be removed from the returned
        //     sequence.
        //
        //   comparer:
        //     An System.Collections.Generic.IEqualityComparer`1 to compare values.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of the input sequences.
        //
        // Returns:
        //     A sequence that contains the set difference of the elements of two sequences.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     first or second is null.
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> Except(BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> second, IEqualityComparer<TEntry> comparer)
        {
            Query = Query.Except(second.Query, comparer);
            return this;
        }

        //
        // Summary:
        //     Produces the set intersection of two sequences by using the default equality
        //     comparer to compare values.
        //
        // Parameters:
        //   first:
        //     An System.Collections.Generic.IEnumerable`1 whose distinct elements that also
        //     appear in second will be returned.
        //
        //   second:
        //     An System.Collections.Generic.IEnumerable`1 whose distinct elements that also
        //     appear in the first sequence will be returned.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of the input sequences.
        //
        // Returns:
        //     A sequence that contains the elements that form the set intersection of two sequences.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     first or second is null.
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> Intersect(BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> second)
        {
            Query = Query.Intersect(second.Query);
            return this;
        }

        //
        // Summary:
        //     Produces the set intersection of two sequences by using the specified System.Collections.Generic.IEqualityComparer`1
        //     to compare values.
        //
        // Parameters:
        //   first:
        //     An System.Collections.Generic.IEnumerable`1 whose distinct elements that also
        //     appear in second will be returned.
        //
        //   second:
        //     An System.Collections.Generic.IEnumerable`1 whose distinct elements that also
        //     appear in the first sequence will be returned.
        //
        //   comparer:
        //     An System.Collections.Generic.IEqualityComparer`1 to compare values.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of the input sequences.
        //
        // Returns:
        //     A sequence that contains the elements that form the set intersection of two sequences.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     first or second is null.
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> Intersect(BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> second, IEqualityComparer<TEntry> comparer)
        {
            Query = Query.Intersect(second.Query, comparer);
            return this;
        }

        //
        // Parameters:
        //   source:
        //
        //   element:
        //
        // Type parameters:
        //   TEntry:
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> Prepend(TEntry element)
        {
            Query = Query.Prepend(element);
            return this;
        }

        //
        // Summary:
        //     Bypasses a specified number of elements in a sequence and then returns the remaining
        //     elements.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to return elements from.
        //
        //   count:
        //     The number of elements to skip before returning the remaining elements.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains the elements that occur
        //     after the specified index in the input sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> Skip(int count)
        {
            Query = Query.Skip(count);
            return this;
        }

        //
        // Parameters:
        //   source:
        //
        //   count:
        //
        // Type parameters:
        //   TEntry:
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> SkipLast(int count)
        {
            Query = Query.SkipLast(count);
            return this;
        }

        //
        // Summary:
        //     Bypasses elements in a sequence as long as a specified condition is true and
        //     then returns the remaining elements. The element's index is used in the logic
        //     of the predicate function.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to return elements from.
        //
        //   predicate:
        //     A function to test each source element for a condition; the second parameter
        //     of the function represents the index of the source element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains the elements from the
        //     input sequence starting at the first element in the linear series that does not
        //     pass the test specified by predicate.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> SkipWhile(Expression<Func<TEntry, int, bool>> predicate)
        {
            Query = Query.SkipWhile(predicate);
            return this;
        }

        //
        // Summary:
        //     Bypasses elements in a sequence as long as a specified condition is true and
        //     then returns the remaining elements.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to return elements from.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains the elements from the
        //     input sequence starting at the first element in the linear series that does not
        //     pass the test specified by predicate.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> SkipWhile(Expression<Func<TEntry, bool>> predicate)
        {
            Query = Query.SkipWhile(predicate);
            return this;
        }

        //
        // Summary:
        //     Returns a specified number of contiguous elements from the start of a sequence.
        //
        // Parameters:
        //   source:
        //     The sequence to return elements from.
        //
        //   count:
        //     The number of elements to return.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains the specified number
        //     of elements from the start of the input sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> Take(int count)
        {
            Query = Query.Take(count);
            return this;
        }

        //
        // Parameters:
        //   source:
        //
        //   count:
        //
        // Type parameters:
        //   TEntry:
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> TakeLast(int count)
        {
            Query = Query.TakeLast(count);
            return this;
        }

        //
        // Summary:
        //     Returns elements from a sequence as long as a specified condition is true.
        //
        // Parameters:
        //   source:
        //     A sequence to return elements from.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains the elements from the
        //     input sequence that occur before the element at which the test no longer passes.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> TakeWhile(Expression<Func<TEntry, bool>> predicate)
        {
            Query = Query.TakeWhile(predicate);
            return this;
        }

        //
        // Summary:
        //     Returns elements from a sequence as long as a specified condition is true. The
        //     element's index is used in the logic of the predicate function.
        //
        // Parameters:
        //   source:
        //     The sequence to return elements from.
        //
        //   predicate:
        //     A function to test each source element for a condition; the second parameter
        //     of the function represents the index of the source element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains elements from the input
        //     sequence that occur before the element at which the test no longer passes.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> TakeWhile(Expression<Func<TEntry, int, bool>> predicate)
        {
            Query = Query.TakeWhile(predicate);
            return this;
        }

        //
        // Summary:
        //     Produces the set union of two sequences by using the default equality comparer.
        //
        // Parameters:
        //   first:
        //     An System.Collections.Generic.IEnumerable`1 whose distinct elements form the
        //     first set for the union.
        //
        //   second:
        //     An System.Collections.Generic.IEnumerable`1 whose distinct elements form the
        //     second set for the union.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of the input sequences.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains the elements from both
        //     input sequences, excluding duplicates.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     first or second is null.
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> Union(BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> second)
        {
            Query = Query.Union(second.Query);
            return this;
        }

        //
        // Summary:
        //     Produces the set union of two sequences by using a specified System.Collections.Generic.IEqualityComparer`1.
        //
        // Parameters:
        //   first:
        //     An System.Collections.Generic.IEnumerable`1 whose distinct elements form the
        //     first set for the union.
        //
        //   second:
        //     An System.Collections.Generic.IEnumerable`1 whose distinct elements form the
        //     second set for the union.
        //
        //   comparer:
        //     The System.Collections.Generic.IEqualityComparer`1 to compare values.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of the input sequences.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains the elements from both
        //     input sequences, excluding duplicates.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     first or second is null.
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> Union(BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> second, IEqualityComparer<TEntry> comparer)
        {
            Query = Query.Union(second.Query, comparer);
            return this;
        }

        //
        // Summary:
        //     Filters a sequence of values based on a predicate. Each element's index is used
        //     in the logic of the predicate function.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to filter.
        //
        //   predicate:
        //     A function to test each source element for a condition; the second parameter
        //     of the function represents the index of the source element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains elements from the input
        //     sequence that satisfy the condition.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> Where(Expression<Func<TEntry, int, bool>> predicate)
        {
            Query = Query.Where(predicate);
            return this;
        }

        //
        // Summary:
        //     Filters a sequence of values based on a predicate.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to filter.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     An System.Collections.Generic.IEnumerable`1 that contains elements from the input
        //     sequence that satisfy the condition.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> Where(Expression<Func<TEntry, bool>> predicate)
        {
            Query = Query.Where(predicate);
            return this;
        }

        public BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> Select(Expression<Func<TEntry, TEntry>> predicate)
        {
            Query = Query.Select(predicate);
            return this;
        }
    }
}
