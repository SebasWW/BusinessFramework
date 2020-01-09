using SebasWW.BusinessFramework;
using SebasWW.BusinessFramework.Query;

namespace MyCompany.MyApp.Query
{
	public static class BusinessQueryExtension
	{
		public static BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> Secure<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>(this BusinessQuery<TCollection, TReadOnlyCollection, TObject, TEntry, TKey> businessQuery)
		where TCollection : BusinessCollection<TObject, TEntry, TKey>
		where TReadOnlyCollection : BusinessReadOnlyCollection<TObject, TEntry, TKey>
		where TObject : BusinessObject<TEntry, TKey>
		where TEntry : class
		{
			return businessQuery.ExecCustomFunction(MyAppBusinessContext.READ_SECURITY_KEY);
		}

	}
}
