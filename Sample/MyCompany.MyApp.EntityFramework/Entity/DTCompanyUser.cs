using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.MyApp.EntityFramework.Entity
{
	public class DTCompanyUser
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int CompanyId { get; set; }

		public int UserId { get; set; }

		public DTCompany Company { get; set; }

		public DTUser User { get; set; }
	}
}
