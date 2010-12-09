using System.Globalization;
using System.Threading;
using NHibernate.Criterion;
using NUnit.Framework;
using uNhAddIns.Criterions;

namespace uNhAddIns.Test.CriterionTest
{
	[TestFixture]
	public class LocalizableRestrictionFixture
	{
		[Test]
		public void ConvertToLikeClause()
		{
			Localizable.ConvertToLikeClause("en", "pi__a", '#').Should().Be.EqualTo("%#en##pi__a#%");
			Localizable.ConvertToLikeClause(new CultureInfo("en"), "pi__a", '#').Should().Be.EqualTo("%#en##pi__a#%");

			// should use the DefaultKeyValueEncloser
			Localizable.ConvertToLikeClause("en", "pi__a").Should().Be.EqualTo("%~en~~pi__a~%");
			Localizable.ConvertToLikeClause(new CultureInfo("en"), "pi__a").Should().Be.EqualTo("%~en~~pi__a~%");

			// with default values
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			
			"pi__a".ToLocalizableLikeClause('#')
				.Should("should use the CurrentCulture").Be.EqualTo("%#en-US##pi__a#%");

			"pi__a".ToLocalizableLikeClause()
				.Should("should use the CurrentCulture and DefaultKeyValueEncloser").Be.EqualTo("%~en-US~~pi__a~%");
		}

		[Test]
		public void LikeWithMatchMode()
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			var c = Localizable.Like("Description", "pi__a", MatchMode.Anywhere);
			c.Should().Be.OfType<LikeExpression>()
				.And.ValueOf.ToString()
				.Should().Be.EqualTo("Description like %~en-US~~%pi__a%~%");
		}
	}
}