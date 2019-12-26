using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.MyApp.EntityFramework.Entity
{
	public class DTCompany
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public IEnumerable<DTCompanyUser> CompanyUsers { get; set; }

		public IEnumerable<DTOrder> Orders1 { get; set; }

		public IEnumerable<DTOrder> Orders2 { get; set; }
	}
}
