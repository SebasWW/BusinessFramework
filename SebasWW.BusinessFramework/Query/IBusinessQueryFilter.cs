using System.Linq;

namespace SebasWW.BusinessFramework.Query
{
	public interface IBusinessQueryFilter<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>
		where TCollection : BusinessCollection<TObject, TEntry, TKey>
		where TReadOnlyCollection : BusinessReadOnlyCollection<TObject, TEntry, TKey>
		where TObject : BusinessObject<TEntry, TKey>
		where TEntry : class
	{
		IQueryable<TEntry> ApplyFilter(IQueryable<TEntry> query, BusinessQueryContext<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> businessQueryContext);
	}
}
