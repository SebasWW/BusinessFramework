using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SebasWW.BusinessFramework.Query
{
	public class BusinessQueryEnumerableResult<Result>
        where Result : class
    {
        protected virtual IQueryable<Result> Query{ get; set;}

        internal BusinessQueryEnumerableResult(IQueryable<Result> query)
        {
            Query = query;
        }

        internal virtual IQueryable<Result> GetQuery()
        {
            return Query;
        }

        public BusinessQueryEnumerableResult<Result> Union(BusinessQueryEnumerableResult<Result> second)
        {
            Query = Query.Union(second.Query);
            return this;
        }

        //
        // Summary:
        //     Creates an array from a System.Collections.Generic.IEnumerable`1.
        //
        // Parameters:
        //   source:
        //     An System.Collections.Generic.IEnumerable`1 to create an array from.
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
        public virtual IEnumerable<Result> ToArray()
        {
            return Query.AsNoTracking().ToArray();
        }

        public async virtual Task<IEnumerable<Result>> ToArrayAsync()
        {
            return await Query.AsNoTracking().ToArrayAsync();
        }
    }
}
