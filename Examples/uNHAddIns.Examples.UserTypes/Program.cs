using System;
using System.Collections;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System.Diagnostics;

namespace NH
{
	class Program
	{
		public static void Main()
		{
			Configuration cfg = new Configuration();
			cfg.Configure("hibernate.cfg.xml");
			SchemaExport exporter = new SchemaExport(cfg);
			exporter.Create(true, true);

			using (ISessionFactory sf = cfg.BuildSessionFactory())
			{				
				using (ISession session = sf.OpenSession())
				{
					Customer c = new Customer();					
					c.Id=1;
					c.FirstName = "Astor";
					c.LastName = "Piazzolla";		
					
					session.Save(c);
					session.Flush();
					session.Evict(c);

					
					Customer customer = session.Get<Customer>(1);
					
					Debug.Assert(customer.FirstName == "Astor");
					Debug.Assert(customer.LastName == "PIAZZOLLA");
				}				
			}
			Console.WriteLine("Done");
			Console.ReadKey(true);
		}
	}
}
