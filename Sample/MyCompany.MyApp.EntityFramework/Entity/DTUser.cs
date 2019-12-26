using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.MyApp.EntityFramework.Entity
{
	public class DTUser
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public IEnumerable<DTCompanyUser> CompanyUsers { get; set; }

		public IEnumerable<DTOrder> Orders { get; set; }
	}
}
