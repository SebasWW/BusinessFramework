using System;
using System.Linq;

namespace SebasWW.BusinessFramework.Query
{
	public class CustomBusinessQueryFilter<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> : IBusinessQueryFilter<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>
		where TCollection : BusinessCollection<TObject, TEntry, TKey>
		where TReadOnlyCollection : BusinessReadOnlyCollection<TObject, TEntry, TKey>
		where TObject : BusinessObject<TEntry, TKey>
		where TEntry : class
	{
		private readonly Func<IQueryable<TEntry>, BusinessQueryContext<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>, IQueryable<TEntry>> func;

		public CustomBusinessQueryFilter(Func<IQueryable<TEntry>, BusinessQueryContext<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>, IQueryable<TEntry>> func)
		{
			this.func = func;
		}

		public IQueryable<TEntry> ApplyFilter(IQueryable<TEntry> query, BusinessQueryContext<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> businessQueryContext)
		{
			return func.Invoke(query, businessQueryContext);
		}
	}
}
