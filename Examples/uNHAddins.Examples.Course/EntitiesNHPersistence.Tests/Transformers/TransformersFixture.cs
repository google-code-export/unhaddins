using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Expressions;
using NHibernate.Transform;
using NUnit.Framework;
using uNhAddIns.Transform;
using YourPrjDomain.Transformers;

namespace uNHAddins.Examples.Course.Tests.Transformers
{
	[TestFixture]
	public class TransformersFixture : TestCase
	{
		protected override IList Mappings
		{
			get { return new string[] {"Transformers.Customer.hbm.xml"}; }
		}

		protected override void OnTearDown()
		{
			using (ISession s = OpenSession())
			{
				s.Delete("from Customer");
				s.Flush();
			}
		}

		protected override void OnSetUp()
		{
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				Customer c = new Customer();

				c.FirstName = "Jorge";
				c.LastName = "Martinez";
				c.Address = "56 Street";
				c.Born = DateTime.Parse("1/1/1995");
				c.Phone = "15-353-3434-34";
				c.Zip = "3335";

				s.Save(c);
				tx.Commit();
			}
		}

		[Test]
		public void SimpleHQL_Transformation()
		{
			using (ISession s = OpenSession())
			{
				IQuery q = s.CreateQuery("select c.FirstName, c.LastName from Customer c");

				q.SetResultTransformer(
					new PositionalToBeanResultTransformer(typeof (FirstNameAndLastName), new string[] {"FirstName", "LastName"}));

				IList<FirstNameAndLastName> result =  q.List<FirstNameAndLastName>();

				Assert.AreEqual(1, result.Count);

				Assert.AreEqual("Jorge",result[0].FirstName);
				Assert.AreEqual("Martinez", result[0].LastName);
			}
		}

		[Test]
		public void SimpleCriteria_Transformation()
		{
			using (ISession s = OpenSession())
			{
				ICriteria crit = s.CreateCriteria(typeof (Customer));

				crit.SetProjection(Projections.ProjectionList()
					.Add(Projections.Property("FirstName"),"FirstName")
					.Add(Projections.Property("LastName"),"LastName"));
				
				crit.SetResultTransformer(new AliasToBeanResultTransformer(typeof(FirstNameAndLastName)));

				IList<FirstNameAndLastName> result = crit.List<FirstNameAndLastName>();

				Assert.AreEqual(1, result.Count);

				Assert.AreEqual("Jorge", result[0].FirstName);
				Assert.AreEqual("Martinez", result[0].LastName);
			}
		}
	}
}