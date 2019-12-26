using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SebasWW.BusinessFramework.Query
{
    public partial class BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>
    {

        //
        // Summary:
        //     Applies an accumulator function over a sequence. The specified seed value is
        //     used as the initial accumulator value, and the specified function is used to
        //     select the result value.
        //
        // Parameters:
        //   source:
        //     A sequence to aggregate over.
        //
        //   seed:
        //     The initial accumulator value.
        //
        //   func:
        //     An accumulator function to invoke on each element.
        //
        //   selector:
        //     A function to transform the final accumulator value into the result value.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        //   TAccumulate:
        //     The type of the accumulator value.
        //
        //   TResult:
        //     The type of the resulting value.
        //
        // Returns:
        //     The transformed final accumulator value.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or func or selector is null.
        public TResult Aggregate<TAccumulate, TResult>(TAccumulate seed, Expression<Func<TAccumulate, TEntry, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector)
        {
            return SecureReadQuery().Aggregate(seed, func, selector);
        }

        //
        // Summary:
        //     Applies an accumulator function over a sequence. The specified seed value is
        //     used as the initial accumulator value.
        //
        // Parameters:
        //   source:
        //     A sequence to aggregate over.
        //
        //   seed:
        //     The initial accumulator value.
        //
        //   func:
        //     An accumulator function to invoke on each element.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        //   TAccumulate:
        //     The type of the accumulator value.
        //
        // Returns:
        //     The final accumulator value.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or func is null.
        public TAccumulate Aggregate<TAccumulate>(TAccumulate seed, Expression<Func<TAccumulate, TEntry, TAccumulate>> func)
        {
            return SecureReadQuery().Aggregate(seed, func);
        }

        //
        // Summary:
        //     Determines whether all the elements of a sequence satisfy a condition.
        //
        // Parameters:
        //   source:
        //     A sequence whose elements to test for a condition.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     true if every element of the source sequence passes the test in the specified
        //     predicate, or if the sequence is empty; otherwise, false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        public async Task<bool> AllAsync(Expression<Func<TEntry, bool>> predicate)
        {
            return await SecureReadQuery().AllAsync(predicate);
        }

        //
        // Summary:
        //     Determines whether a sequence contains any elements.
        //
        // Parameters:
        //   source:
        //     A sequence to check for being empty.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     true if the source sequence contains any elements; otherwise, false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public async Task<bool> AnyAsync()
        {
            return await SecureReadQuery().AnyAsync();
        }

        //
        // Summary:
        //     Determines whether any element of a sequence satisfies a condition.
        //
        // Parameters:
        //   source:
        //     A sequence whose elements to test for a condition.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     true if any elements in the source sequence pass the test in the specified predicate;
        //     otherwise, false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        public async Task<bool> AnyAsync(Expression<Func<TEntry, bool>> predicate)
        {
            return await SecureReadQuery().AnyAsync(predicate);
        }

        //
        // Summary:
        //     Computes the average of a sequence of System.Int32 values that is obtained by
        //     invoking a projection function on each element of the input sequence.
        //
        // Parameters:
        //
        //   selector:
        //     A projection function to apply to each element.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The average of the sequence of values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.InvalidOperationException:
        //     source contains no elements.
        public async Task<double> AverageAsync(Expression<Func<TEntry, int>> selector)
        {
            return await SecureReadQuery().AverageAsync(selector);
        }

        //
        // Summary:
        //     Computes the average of a sequence of System.Int64 values that is obtained by
        //     invoking a projection function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values to calculate the average of.
        //
        //   selector:
        //     A projection function to apply to each element.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The average of the sequence of values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.InvalidOperationException:
        //     source contains no elements.
        public async Task<double> AverageAsync(Expression<Func<TEntry, long>> selector)
        {
            return await SecureReadQuery().AverageAsync(selector);
        }

        //
        // Summary:
        //     Computes the average of a sequence of nullable System.Decimal values that is
        //     obtained by invoking a projection function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values to calculate the average of.
        //
        //   selector:
        //     A projection function to apply to each element.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The average of the sequence of values, or null if the source sequence is empty
        //     or contains only null values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public async Task<decimal?> AverageAsync(Expression<Func<TEntry, decimal?>> selector)
        {
            return await SecureReadQuery().AverageAsync(selector);
        }

        //
        // Summary:
        //     Computes the average of a sequence of nullable System.Double values that is obtained
        //     by invoking a projection function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values to calculate the average of.
        //
        //   selector:
        //     A projection function to apply to each element.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The average of the sequence of values, or null if the source sequence is empty
        //     or contains only null values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public async Task<double?> AverageAsync(Expression<Func<TEntry, double?>> selector)
        {
            return await SecureReadQuery().AverageAsync(selector);
        }

        //
        // Summary:
        //     Computes the average of a sequence of System.Single values that is obtained by
        //     invoking a projection function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values to calculate the average of.
        //
        //   selector:
        //     A projection function to apply to each element.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The average of the sequence of values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.InvalidOperationException:
        //     source contains no elements.
        public async Task<float> AverageAsync(Expression<Func<TEntry, float>> selector)
        {
            return await SecureReadQuery().AverageAsync(selector);
        }


        //
        // Summary:
        //     Computes the average of a sequence of nullable System.Int64 values that is obtained
        //     by invoking a projection function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values to calculate the average of.
        //
        //   selector:
        //     A projection function to apply to each element.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The average of the sequence of values, or null if the source sequence is empty
        //     or contains only null values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public async Task<double?> AverageAsync(Expression<Func<TEntry, long?>> selector)
        {
            return await SecureReadQuery().AverageAsync(selector);
        }

        //
        // Summary:
        //     Computes the average of a sequence of nullable System.Single values that is obtained
        //     by invoking a projection function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values to calculate the average of.
        //
        //   selector:
        //     A projection function to apply to each element.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The average of the sequence of values, or null if the source sequence is empty
        //     or contains only null values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public async Task<float?> AverageAsync(Expression<Func<TEntry, float?>> selector)
        {
            return await SecureReadQuery().AverageAsync(selector);
        }

        //
        // Summary:
        //     Computes the average of a sequence of System.Double values that is obtained by
        //     invoking a projection function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values to calculate the average of.
        //
        //   selector:
        //     A projection function to apply to each element.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The average of the sequence of values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.InvalidOperationException:
        //     source contains no elements.
        public async Task<double> AverageAsync(Expression<Func<TEntry, double>> selector)
        {
            return await SecureReadQuery().AverageAsync(selector);
        }

        //
        // Summary:
        //     Computes the average of a sequence of nullable System.Int32 values that is obtained
        //     by invoking a projection function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values to calculate the average of.
        //
        //   selector:
        //     A projection function to apply to each element.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The average of the sequence of values, or null if the source sequence is empty
        //     or contains only null values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public async Task<double?> AverageAsync(Expression<Func<TEntry, int?>> selector)
        {
            return await SecureReadQuery().AverageAsync(selector);
        }

        //
        // Summary:
        //     Computes the average of a sequence of System.Decimal values that is obtained
        //     by invoking a projection function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate an average.
        //
        //   selector:
        //     A projection function to apply to each element.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The average of the sequence of values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.InvalidOperationException:
        //     source contains no elements.
        public async Task<decimal> AverageAsync(Expression<Func<TEntry, decimal>> selector)
        {
            return await SecureReadQuery().AverageAsync(selector);
        }

        //
        // Summary:
        //     Determines whether a sequence contains a specified element by using the default
        //     equality comparer.
        //
        // Parameters:
        //   source:
        //     A sequence in which to locate a value.
        //
        //   value:
        //     The value to locate in the sequence.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     true if the source sequence contains an element that has the specified value;
        //     otherwise, false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public async Task<bool> ContainsAsync(TEntry value)
        {
            return await SecureReadQuery().ContainsAsync(value);
        }

        //
        // Summary:
        //     Determines whether a sequence contains a specified element by using a specified
        //     System.Collections.Generic.IEqualityComparer`1.
        //
        // Parameters:
        //   source:
        //     A sequence in which to locate a value.
        //
        //   value:
        //     The value to locate in the sequence.
        //
        //   comparer:
        //     An equality comparer to compare values.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     true if the source sequence contains an element that has the specified value;
        //     otherwise, false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        public bool Contains(TEntry value, IEqualityComparer<TEntry> comparer)
        {
            return SecureReadQuery().Contains( value, comparer);
        }

        //
        // Summary:
        //     Returns a number that represents how many elements in the specified sequence
        //     satisfy a condition.
        //
        // Parameters:
        //   source:
        //     A sequence that contains elements to be tested and counted.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     A number that represents how many elements in the sequence satisfy the condition
        //     in the predicate function.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        //
        //   T:System.OverflowException:
        //     The number of elements in source is larger than System.Int32.MaxValue.
        public async Task<int> CountAsync(Expression<Func<TEntry, bool>> predicate)
        {
            return await SecureReadQuery().CountAsync(predicate);
        }

        //
        // Summary:
        //     Returns the number of elements in a sequence.
        //
        // Parameters:
        //   source:
        //     A sequence that contains elements to be counted.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The number of elements in the input sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        //
        //   T:System.OverflowException:
        //     The number of elements in source is larger than System.Int32.MaxValue.
        public async Task<int> CountAsync()
        {
            return await SecureReadQuery().CountAsync();
        }

        //
        // Summary:
        //     Returns an System.Int64 that represents the total number of elements in a sequence.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 that contains the elements to be
        //     counted.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     The number of elements in the source sequence.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source is null.
        //
        //   T:System.OverflowException:
        //     The number of elements exceeds System.Int64.MaxValue.
        public async Task<Int64> LongCountAsync()
        {
            return await SecureReadQuery().LongCountAsync();
        }

        //
        // Summary:
        //     Returns an System.Int64 that represents how many elements in a sequence satisfy
        //     a condition.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 that contains the elements to be
        //     counted.
        //
        //   predicate:
        //     A function to test each element for a condition.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements of source.
        //
        // Returns:
        //     A number that represents how many elements in the sequence satisfy the condition
        //     in the predicate function.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or predicate is null.
        //
        //   T:System.OverflowException:
        //     The number of matching elements exceeds System.Int64.MaxValue.
        public async Task<Int64> LongCountAsync(Expression<Func<TEntry, bool>> predicate)
        {
            return await SecureReadQuery().LongCountAsync(predicate);
        }

        //
        // Summary:
        //     Computes the max of the sequence of System.Double values that are obtained by
        //     invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a max.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The max of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public async Task<double> MaxAsync(Expression<Func<TEntry, double>> selector)
        {
            return await SecureReadQuery().MaxAsync(selector);
        }

        //
        // Summary:
        //     Computes the max of the sequence of nullable System.Single values that are obtained
        //     by invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a max.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The max of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public async Task<float?> MaxAsync(Expression<Func<TEntry, float?>> selector)
        {
            return await SecureReadQuery().MaxAsync(selector);
        }

        //
        // Summary:
        //     Computes the max of the sequence of nullable System.Int64 values that are obtained
        //     by invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a max.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The max of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.OverflowException:
        //     The max is larger than System.Int64.MaxValue.
        public async Task<long?> MaxAsync(Expression<Func<TEntry, long?>> selector)
        {
            return await SecureReadQuery().MaxAsync(selector);
        }

        //
        // Summary:
        //     Computes the max of the sequence of nullable System.Int32 values that are obtained
        //     by invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a max.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The max of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.OverflowException:
        //     The max is larger than System.Int32.MaxValue.
        public async Task<int?> MaxAsync(Expression<Func<TEntry, int?>> selector)
        {
            return await SecureReadQuery().MaxAsync(selector);
        }

        //
        // Summary:
        //     Computes the max of the sequence of nullable System.Double values that are obtained
        //     by invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a max.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The max of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public async Task<double?> MaxAsync(Expression<Func<TEntry, double?>> selector)
        {
            return await SecureReadQuery().MaxAsync(selector);
        }

        //
        // Summary:
        //     Computes the max of the sequence of System.Single values that are obtained by
        //     invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a max.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The max of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public async Task<float> MaxAsync(Expression<Func<TEntry, float>> selector)
        {
            return await SecureReadQuery().MaxAsync(selector);
        }

        //
        // Summary:
        //     Computes the max of the sequence of nullable System.Decimal values that are obtained
        //     by invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a max.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The max of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.OverflowException:
        //     The max is larger than System.Decimal.MaxValue.
        public async Task<decimal?> MaxAsync(Expression<Func<TEntry, decimal?>> selector)
        {
            return await SecureReadQuery().MaxAsync(selector);
        }

        //
        // Summary:
        //     Computes the max of the sequence of System.Int64 values that are obtained by
        //     invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a max.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The max of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.OverflowException:
        //     The max is larger than System.Int64.MaxValue.
        public async Task<long> MaxAsync(Expression<Func<TEntry, long>> selector)
        {
            return await SecureReadQuery().MaxAsync(selector);
        }

        //
        // Summary:
        //     Computes the max of the sequence of System.Int32 values that are obtained by
        //     invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a max.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The max of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.OverflowException:
        //     The max is larger than System.Int32.MaxValue.
        public async Task<int> MaxAsync(Expression<Func<TEntry, int>> selector)
        {
            return await SecureReadQuery().MaxAsync(selector);
        }

        //
        // Summary:
        //     Computes the max of the sequence of System.Decimal values that are obtained by
        //     invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a max.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The max of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.OverflowException:
        //     The max is larger than System.Decimal.MaxValue.
        public async Task<decimal> MaxAsync(Expression<Func<TEntry, decimal>> selector)
        {
            return await SecureReadQuery().MaxAsync(selector);
        }

        //
        // Summary:
        //     Computes the min of the sequence of System.Double values that are obtained by
        //     invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a min.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The min of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public async Task<double> MinAsync(Expression<Func<TEntry, double>> selector)
        {
            return await SecureReadQuery().MinAsync(selector);
        }

        //
        // Summary:
        //     Computes the min of the sequence of nullable System.Single values that are obtained
        //     by invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a min.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The min of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public async Task<float?> MinAsync(Expression< Func<TEntry, float?>> selector)
        {
            return await SecureReadQuery().MinAsync(selector);
        }

        //
        // Summary:
        //     Computes the min of the sequence of nullable System.Int64 values that are obtained
        //     by invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a min.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The min of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.OverflowException:
        //     The min is larger than System.Int64.MaxValue.
        public async Task<long?> MinAsync(Expression<Func<TEntry, long?>> selector)
        {
            return await SecureReadQuery().MinAsync(selector);
        }

        //
        // Summary:
        //     Computes the min of the sequence of nullable System.Int32 values that are obtained
        //     by invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a min.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The min of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.OverflowException:
        //     The min is larger than System.Int32.MaxValue.
        public async Task<int?> MinAsync(Expression<Func<TEntry, int?>> selector)
        {
            return await SecureReadQuery().MinAsync(selector);
        }

        //
        // Summary:
        //     Computes the min of the sequence of nullable System.Double values that are obtained
        //     by invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a min.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The min of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public async Task<double?> MinAsync(Expression<Func<TEntry, double?>> selector)
        {
            return await SecureReadQuery().MinAsync(selector);
        }

        //
        // Summary:
        //     Computes the min of the sequence of System.Single values that are obtained by
        //     invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a min.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The min of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public async Task<float> MinAsync(Expression<Func<TEntry, float>> selector)
        {
            return await SecureReadQuery().MinAsync(selector);
        }

        //
        // Summary:
        //     Computes the min of the sequence of nullable System.Decimal values that are obtained
        //     by invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a min.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The min of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.OverflowException:
        //     The min is larger than System.Decimal.MaxValue.
        public async Task<decimal?> MinAsync(Expression<Func<TEntry, decimal?>> selector)
        {
            return await SecureReadQuery().MinAsync(selector);
        }

        //
        // Summary:
        //     Computes the min of the sequence of System.Int64 values that are obtained by
        //     invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a min.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The min of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.OverflowException:
        //     The min is larger than System.Int64.MaxValue.
        public async Task<long> MinAsync(Expression<Func<TEntry, long>> selector)
        {
            return await SecureReadQuery().MinAsync(selector);
        }

        //
        // Summary:
        //     Computes the min of the sequence of System.Int32 values that are obtained by
        //     invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a min.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The min of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.OverflowException:
        //     The min is larger than System.Int32.MaxValue.
        public async Task<int> MinAsync(Expression<Func<TEntry, int>> selector)
        {
            return await SecureReadQuery().MinAsync(selector);
        }

        //
        // Summary:
        //     Computes the min of the sequence of System.Decimal values that are obtained by
        //     invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a min.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The min of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.OverflowException:
        //     The min is larger than System.Decimal.MaxValue.
        public async Task<decimal> MinAsync(Expression<Func<TEntry, decimal>> selector)
        {
            return await SecureReadQuery().MinAsync(selector);
        }

        //
        // Summary:
        //     Computes the sum of the sequence of System.Double values that are obtained by
        //     invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a sum.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The sum of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public async Task<double> SumAsync(Expression<Func<TEntry, double>> selector)
        {
            return await SecureReadQuery().SumAsync(selector);
        }

        //
        // Summary:
        //     Computes the sum of the sequence of nullable System.Single values that are obtained
        //     by invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a sum.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The sum of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public async Task<double?> SumAsync(Expression<Func<TEntry, float?>> selector)
        {
            return await SecureReadQuery().SumAsync(selector);
        }

        //
        // Summary:
        //     Computes the sum of the sequence of nullable System.Int64 values that are obtained
        //     by invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a sum.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The sum of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.OverflowException:
        //     The sum is larger than System.Int64.MaxValue.
        public async Task<Int64?> SumAsync(Expression<Func<TEntry, long?>> selector)
        {
            return await SecureReadQuery().SumAsync(selector);
        }

        //
        // Summary:
        //     Computes the sum of the sequence of nullable System.Int32 values that are obtained
        //     by invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a sum.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The sum of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.OverflowException:
        //     The sum is larger than System.Int32.MaxValue.
        public async Task<Int32?> SumAsync(Expression<Func<TEntry, int?>> selector)
        {
            return await SecureReadQuery().SumAsync(selector);
        }

        //
        // Summary:
        //     Computes the sum of the sequence of nullable System.Double values that are obtained
        //     by invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a sum.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The sum of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public async Task<double?> SumAsync(Expression<Func<TEntry, double?>> selector)
        {
            return await SecureReadQuery().SumAsync(selector);
        }

        //
        // Summary:
        //     Computes the sum of the sequence of System.Single values that are obtained by
        //     invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a sum.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The sum of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        public async Task<double> SumAsync(Expression<Func<TEntry, float>> selector)
        {
            return await SecureReadQuery().SumAsync(selector);
        }

        //
        // Summary:
        //     Computes the sum of the sequence of nullable System.Decimal values that are obtained
        //     by invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a sum.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The sum of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.OverflowException:
        //     The sum is larger than System.Decimal.MaxValue.
        public async Task<decimal?> SumAsync(Expression<Func<TEntry, decimal?>> selector)
        {
            return await SecureReadQuery().SumAsync(selector);
        }

        //
        // Summary:
        //     Computes the sum of the sequence of System.Int64 values that are obtained by
        //     invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a sum.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The sum of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.OverflowException:
        //     The sum is larger than System.Int64.MaxValue.
        public async Task<Int64> SumAsync(Expression<Func<TEntry, long>> selector)
        {
            return await SecureReadQuery().SumAsync(selector);
        }

        //
        // Summary:
        //     Computes the sum of the sequence of System.Int32 values that are obtained by
        //     invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a sum.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The sum of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.OverflowException:
        //     The sum is larger than System.Int32.MaxValue.
        public async Task<Int32> SumAsync(Expression<Func<TEntry, int>> selector)
        {
            return await SecureReadQuery().SumAsync(selector);
        }

        //
        // Summary:
        //     Computes the sum of the sequence of System.Decimal values that are obtained by
        //     invoking a transform function on each element of the input sequence.
        //
        // Parameters:
        //   source:
        //     A sequence of values that are used to calculate a sum.
        //
        //   selector:
        //     A transform function to apply to each element.
        //
        // Type parameters:
        //   TEntry:
        //     The type of the elements of source.
        //
        // Returns:
        //     The sum of the projected values.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or selector is null.
        //
        //   T:System.OverflowException:
        //     The sum is larger than System.Decimal.MaxValue.
        public async Task<decimal> SumAsync(Expression<Func<TEntry, decimal>> selector)
        {
            return await SecureReadQuery().SumAsync(selector);
        }
    }
}
