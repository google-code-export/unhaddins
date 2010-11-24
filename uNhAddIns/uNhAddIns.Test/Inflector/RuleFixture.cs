using System;
using NUnit.Framework;
using uNhAddIns.Inflector;

namespace uNhAddIns.Test.Inflector
{
	[TestFixture]
	public class RuleFixture
	{
		[Test]
		public void Ctor()
		{
			Assert.Throws<ArgumentNullException>(() => new NounsRule(null, null));
			Assert.Throws<ArgumentNullException>(() => new NounsRule("$s", null));
			Assert.Throws<ArgumentNullException>(() => new NounsRule(null, "s"));
			Assert.Throws<ArgumentNullException>(() => new NounsRule("", "s"));
			Assert.DoesNotThrow(() => new NounsRule("$s", ""));
		}

		[Test]
		public void Equality()
		{
			var r = new NounsRule("$s", "s");
			r.Should().Be.EqualTo(new NounsRule("$s", "s"));
			r.Should().Not.Be.EqualTo(new NounsRule("$s", "ss"));
		}

		[Test]
		public void HashCode()
		{
			var r = new NounsRule("$s", "s");
			r.GetHashCode().Should().Be.EqualTo((new NounsRule("$s", "s")).GetHashCode());
			r.GetHashCode().Should().Not.Be.EqualTo((new NounsRule("$s", "ss")).GetHashCode());
		}

		[Test]
		public void Apply()
		{
			var r = new NounsRule("(pizz)a$", "$1e");
			r.Apply("pizza").Should().Be.EqualTo("pizze");
		}
	}
}