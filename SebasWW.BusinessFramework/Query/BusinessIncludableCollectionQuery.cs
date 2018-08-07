using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace SebasWW.BusinessFramework.Query
{
    public class BusinessIncludableCollectionQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey, TProperty> : BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>
        where TCollection : GenericCollection<TObject, TEntry, TKey>
        where TReadOnlyCollection : GenericReadOnlyCollection<TObject, TEntry, TKey>
        where TObject : GenericObject<TEntry, TKey>
        where TEntry : class
    {
        IIncludableQueryable<TEntry, IEnumerable<TProperty>> _includableQueryable;

        public BusinessIncludableCollectionQuery(
            BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> businessQuery,
            IIncludableQueryable<TEntry, IEnumerable<TProperty>> query
            ) : base(businessQuery.BusinessQueryContext,query)
        {
            _includableQueryable = query;
        }

        //
        // Summary:
        //     Specifies additional related data to be further included based on a related type
        //     that was just included.
        //
        // Parameters:
        //   source:
        //     The source query.
        //
        //   navigationPropertyPath:
        //     A lambda expression representing the navigation property to be included (t =>
        //     t.Property1).
        //
        // Type parameters:
        //   TEntity:
        //     The type of entity being queried.
        //
        //   TPreviousProperty:
        //     The type of the entity that was just included.
        //
        //   TProperty:
        //     The type of the related entity to be included.
        //
        // Returns:
        //     A new query with the related data included.
        public BusinessIncludableQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey, TNewProperty>
            ThenInclude<TNewProperty>(Expression<Func<TProperty, TNewProperty>> navigationPropertyPath) where TNewProperty : class
        {
            return new BusinessIncludableQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey, TNewProperty>
                (
                    this,
                    _includableQueryable.ThenInclude<TEntry, TProperty, TNewProperty>(navigationPropertyPath)
                 );
        }

        //
        // Summary:
        //     Specifies additional related data to be further included based on a related type
        //     that was just included.
        //
        // Parameters:
        //   source:
        //     The source query.
        //
        //   navigationPropertyPath:
        //     A lambda expression representing the navigation property to be included (t =>
        //     t.Property1).
        //
        // Type parameters:
        //   TEntity:
        //     The type of entity being queried.
        //
        //   TPreviousProperty:
        //     The type of the entity that was just included.
        //
        //   TProperty:
        //     The type of the related entity to be included.
        //
        // Returns:
        //     A new query with the related data included.
        public BusinessIncludableCollectionQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey, TNewProperty>
            ThenInclude<TNewProperty>(Expression<Func<TProperty, ICollection<TNewProperty>>> navigationPropertyPath) where TNewProperty : class
        {
            return new BusinessIncludableCollectionQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey, TNewProperty>
                (
                    this,
                    _includableQueryable.ThenInclude<TEntry, TProperty, ICollection<TNewProperty>>(navigationPropertyPath)
                 );
        }
    }
}
