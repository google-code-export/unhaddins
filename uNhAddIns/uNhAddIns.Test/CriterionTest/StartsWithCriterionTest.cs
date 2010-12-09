using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Impl;
using NHibernate.Loader.Criteria;
using NHibernate.SqlCommand;
using NHibernate.Util;
using NUnit.Framework;
using uNhAddIns.Criterions;

namespace uNhAddIns.Test.CriterionTest
{
	[TestFixture]
	public class StartsWithCriterionTest : TestCase
	{
		protected override IList<string> Mappings
		{
			get { return new[] {"CriterionTest.Simple.hbm.xml"}; }
		}

		[Test]
		public void StartsWithTest()
		{
			using (ISession session = OpenSession())
			{
				var criteria = (CriteriaImpl) session.CreateCriteria(typeof (Simple));
				var criteriaQuery = new CriteriaQueryTranslator(sessions, criteria, criteria.EntityOrClassName, "sql_alias");

				ICriterion exp = Criterion.StartsWith("Number", "2");
				SqlString sqlString = exp.ToSqlString(criteria, criteriaQuery, new CollectionHelper.EmptyMapClass<string, IFilter>());

				string expectedSql = "sql_alias.Number like ?";

				Assert.AreEqual(expectedSql, sqlString.ToString());
				Assert.AreEqual(1, sqlString.GetParameterCount());
			}
		}
	}
}