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
	public class CriterionTest : TestCase
	{
		protected override IList<string> Mappings
		{
			get { return new[] {"CriterionTest.Simple.hbm.xml"}; }
		}

		[Test]
		public void EqOrNullTest()
		{
			using (ISession session = OpenSession())
			{
				var criteria = (CriteriaImpl) session.CreateCriteria(typeof (Simple));
				var criteriaQuery = new CriteriaQueryTranslator(sessions, criteria, criteria.EntityOrClassName, "sql_alias");

				ICriterion exp = Criterion.EqOrNull("Name", "foo");
				SqlString sqlString = exp.ToSqlString(criteria, criteriaQuery, new CollectionHelper.EmptyMapClass<string, IFilter>());

				string expectedSql = "sql_alias.Name = ?";

				Assert.AreEqual(expectedSql, sqlString.ToString());
				Assert.AreEqual(1, sqlString.GetParameterCount());

				exp = Criterion.EqOrNull("Name", null);
				sqlString = exp.ToSqlString(criteria, criteriaQuery, new CollectionHelper.EmptyMapClass<string, IFilter>());

				expectedSql = "sql_alias.Name is null";

				Assert.AreEqual(expectedSql, sqlString.ToString());
				Assert.AreEqual(0, sqlString.GetParameterCount());

				// Check that the result is the same than using official Restriction
				ICriterion orExpExpected = Restrictions.Or(Restrictions.IsNull("Name"), Restrictions.Eq("Name", "foo"));
				ICriterion orExpActual = Restrictions.Or(Criterion.EqOrNull("Name", null), Criterion.EqOrNull("Name", "foo"));

				SqlString sqlStringExpected = orExpExpected.ToSqlString(criteria, criteriaQuery,
				                                                        new CollectionHelper.EmptyMapClass<string, IFilter>());
				SqlString sqlStringActual = orExpActual.ToSqlString(criteria, criteriaQuery,
				                                                    new CollectionHelper.EmptyMapClass<string, IFilter>());
				Assert.AreEqual(sqlStringExpected.ToString(), sqlStringActual.ToString());
			}
		}

		[Test]
		public void EqOrNullShouldWorkAsOfficialRestriction()
		{
			const string propertyName = "name";
			const string propertyValue = "something";

			var equalsOrNullWinthNull = new EqOrNullExpression(propertyName, null);
			var equalsOrNullWinthValue = new EqOrNullExpression(propertyName, propertyValue);
			var nullExpression = new NullExpression(propertyName);
			var equalsExpression = new SimpleExpression(propertyName, propertyValue, " = ", false);

			Assert.That(equalsOrNullWinthNull.ToString(), Is.EqualTo(nullExpression.ToString()));
			IEnumerable<IProjection> actual = equalsOrNullWinthNull.GetProjections();
			IEnumerable<IProjection> expected = nullExpression.GetProjections();
			AssertAreEquivalent(actual, expected);

			Assert.That(equalsOrNullWinthValue.ToString(), Is.EqualTo(equalsExpression.ToString()));
			actual = equalsOrNullWinthNull.GetProjections();
			expected = equalsExpression.GetProjections();
			AssertAreEquivalent(actual, expected);
		}

		private static void AssertAreEquivalent(IEnumerable<IProjection> actual, IEnumerable<IProjection> expected)
		{
			if (actual == null)
			{
				Assert.That(expected, Is.Null);
			}
			else
			{
				Assert.That(new List<IProjection>(actual), Is.EquivalentTo(expected));
			}
		}
	}
}