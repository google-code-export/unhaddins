using System.Collections.Generic;
using NUnit.Framework;
using uNhAddIns.Inflector;

namespace uNhAddIns.Test.Inflector
{
	public abstract class BaseInflectorFixture
	{
		public readonly Dictionary<string, string> SingularToPlural = new Dictionary<string, string>();
		public readonly Dictionary<string, string> PascalToUnderscore = new Dictionary<string, string>();
		public readonly Dictionary<string, string> UnderscoreToCamel = new Dictionary<string, string>();

		public readonly Dictionary<string, string> PascalToUnderscoreWithoutReverse =
			new Dictionary<string, string>();

		public readonly Dictionary<string, string> CamelWithModuleToUnderscoreWithSlash =
			new Dictionary<string, string>();

		public readonly Dictionary<string, string> UnderscoreToHuman = new Dictionary<string, string>();
		public readonly Dictionary<string, string> MixtureToTitleCase = new Dictionary<string, string>();
		public readonly Dictionary<string, string> OrdinalNumbers = new Dictionary<string, string>();
		public readonly Dictionary<string, string> UnderscoresToDashes = new Dictionary<string, string>();
		public readonly Dictionary<string, string> ClassNameToTableName = new Dictionary<string, string>();
		public readonly Dictionary<string, string> ClassNameToForeignKeyName = new Dictionary<string, string>();

		public IInflector TestInflector { get; set; }


		[Test]
		public void Pluralize()
		{
			foreach (KeyValuePair<string, string> keyValuePair in SingularToPlural)
			{
				Assert.AreEqual(keyValuePair.Value, TestInflector.Pluralize(keyValuePair.Key));
				Assert.AreEqual(TestInflector.Capitalize(keyValuePair.Value),
												TestInflector.Pluralize(TestInflector.Capitalize(keyValuePair.Key)));
			}
		}

		[Test]
		public void Singularize()
		{
			foreach (KeyValuePair<string, string> keyValuePair in SingularToPlural)
			{
				Assert.AreEqual(keyValuePair.Key, TestInflector.Singularize(keyValuePair.Value));
				Assert.AreEqual(TestInflector.Capitalize(keyValuePair.Key),
												TestInflector.Singularize(TestInflector.Capitalize(keyValuePair.Value)));
			}
		}

		[Test]
		public void TitleCase()
		{
			foreach (KeyValuePair<string, string> keyValuePair in MixtureToTitleCase)
			{
				Assert.AreEqual(keyValuePair.Value, TestInflector.Titleize(keyValuePair.Key));
			}
		}

		[Test]
		public void Pascalize()
		{
			foreach (KeyValuePair<string, string> keyValuePair in PascalToUnderscore)
			{
				Assert.AreEqual(keyValuePair.Key, TestInflector.Pascalize(keyValuePair.Value));
			}
		}

		[Test]
		public void Camelize()
		{
			foreach (KeyValuePair<string, string> keyValuePair in UnderscoreToCamel)
			{
				Assert.AreEqual(keyValuePair.Value, TestInflector.Camelize(keyValuePair.Key));
			}
		}

		[Test]
		public void Underscore()
		{
			foreach (KeyValuePair<string, string> keyValuePair in PascalToUnderscore)
			{
				Assert.AreEqual(keyValuePair.Value, TestInflector.Underscore(keyValuePair.Key));
			}

			foreach (KeyValuePair<string, string> keyValuePair in PascalToUnderscoreWithoutReverse)
			{
				Assert.AreEqual(keyValuePair.Value, TestInflector.Underscore(keyValuePair.Key));
			}

			foreach (KeyValuePair<string, string> keyValuePair in UnderscoreToCamel)
			{
				Assert.AreEqual(keyValuePair.Key, TestInflector.Underscore(keyValuePair.Value));
			}

			foreach (KeyValuePair<string, string> keyValuePair in UnderscoreToHuman)
			{
				Assert.AreEqual(keyValuePair.Key, TestInflector.Underscore(keyValuePair.Value));
			}
		}

		[Test]
		public void Humanize()
		{
			foreach (KeyValuePair<string, string> keyValuePair in UnderscoreToHuman)
			{
				Assert.AreEqual(keyValuePair.Value, TestInflector.Humanize(keyValuePair.Key));
			}
		}

		[Test]
		public void Ordinal()
		{
			foreach (KeyValuePair<string, string> keyValuePair in OrdinalNumbers)
			{
				Assert.AreEqual(keyValuePair.Value, TestInflector.Ordinalize(keyValuePair.Key));
			}
		}

		[Test]
		public void Dasherize()
		{
			foreach (KeyValuePair<string, string> keyValuePair in UnderscoresToDashes)
			{
				Assert.AreEqual(keyValuePair.Value, TestInflector.Dasherize(keyValuePair.Key));
			}
		}

		[Test]
		public void Unaccent()
		{
			TestInflector.Unaccent("¿¡¬√ƒ≈∆«»… ÀÃÕŒœ–—“”‘’÷ÿŸ⁄€‹›ﬁﬂ‡·‚„‰ÂÊÁËÈÍÎÏÌÓÔÒÚÛÙıˆ¯˘˙˚¸˝˛ˇ")
				.Should().Be.EqualTo("AAAAAAACEEEEIIIIDNOOOOOOUUUUYTsaaaaaaaceeeeiiiienoooooouuuuyty");
		}

		[Test]
		public void Tableize()
		{
			foreach (KeyValuePair<string, string> keyValuePair in ClassNameToTableName)
			{
				Assert.AreEqual(keyValuePair.Key, TestInflector.Tableize(keyValuePair.Value));
			}
		}

		[Test]
		public void ForeignKey()
		{
			foreach (KeyValuePair<string, string> keyValuePair in ClassNameToForeignKeyName)
			{
				Assert.AreEqual(keyValuePair.Key, TestInflector.ForeignKey(keyValuePair.Value, false));
			}
		}
	}
}