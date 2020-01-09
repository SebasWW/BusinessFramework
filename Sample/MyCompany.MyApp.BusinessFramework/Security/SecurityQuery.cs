using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyCompany.MyApp.Entity;
using MyCompany.MyApp.EntityFramework.Entity;
using SebasWW.BusinessFramework.Query;

namespace MyCompany.MyApp.Security
{
	//Func<IQueryable<TEntry>, BusinessQueryContext<TCollection, TReadOnlyCollection, TObject, TEntry, TKey>, IQueryable<TEntry>

	public static class ReadSecurityQuery
	{
		private static string OrderQueryExecuted = "OrderQueryExecuted";
		public static IQueryable<DTOrder> Order(IQueryable<DTOrder> query, BusinessQueryContext<OrderCollection, OrderReadOnlyCollection, Order, DTOrder, int> context)
		{
			if (!context.Properties.ContainsKey(OrderQueryExecuted))
			{
				query = query.Where(t => (context.BusinessContext as MyAppBusinessContext).User.Name == "admin");
				context.Properties.Add(OrderQueryExecuted, null);
			}
			
			return query;
		}
	}
}
