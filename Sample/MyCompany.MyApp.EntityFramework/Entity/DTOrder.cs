using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.MyApp.EntityFramework.Entity
{
	public class DTOrder
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int UserId { get; set; }

		public int Company1Id { get; set; }

		public int Company2Id { get; set; }

		public DTUser User { get; set; }

		public DTCompany Company1 { get; set; }
		public DTCompany Company2 { get; set; }
	}
}
