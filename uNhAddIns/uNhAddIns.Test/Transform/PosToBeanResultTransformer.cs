using System;
using NHibernate;
using NUnit.Framework;
using uNhAddIns.Transform;

namespace uNhAddIns.Test.Transform
{
	[TestFixture]
	public class PosToBeanResultTransformer
	{
		private class ASimplePOCO
		{
			private int _intProp;
			public int IntProp
			{
				get { return _intProp; }
				set { _intProp = value; }
			}

			private string _stringProp;
			public string StringProp
			{
				get { return _stringProp; }
				set { _stringProp = value; }
			}

			public string NoSetterProp
			{
				get { return _stringProp; }
			}
		}

		[Test]
		public void ConstructorInvalidType()
		{
			Assert.Throws<ArgumentNullException>(() => new PositionalToBeanResultTransformer(null, new[] {"a", "b"}));
		}

		[Test]
		public void ConstructorInvalidAliases()
		{
			Assert.Throws<ArgumentNullException>(
				() => new PositionalToBeanResultTransformer(typeof (ASimplePOCO), new string[] {}));
		}

		[Test]
		public void Setters()
		{
			var aliases = new[] {"_intProp", "_stringProp"};
			var propAliases = new[] {"IntProp", "StringProp"};

			// Test with field
			var t = new PositionalToBeanResultTransformer(typeof (ASimplePOCO), aliases);
			var asp = (ASimplePOCO) t.TransformTuple(new object[] {1, "test"}, aliases);
			Assert.AreEqual(1, asp.IntProp);
			Assert.AreEqual("test", asp.StringProp);

			// Test with properties
			t = new PositionalToBeanResultTransformer(typeof (ASimplePOCO), propAliases);
			asp = (ASimplePOCO) t.TransformTuple(new object[] {1, "test"}, propAliases);
			Assert.AreEqual(1, asp.IntProp);
			Assert.AreEqual("test", asp.StringProp);
		}

		[Test]
		public void TupleDifferentScalars()
		{
			var aliases = new[] {"_intProp", "_stringProp"};
			var t = new PositionalToBeanResultTransformer(typeof (ASimplePOCO), aliases);
			Assert.Throws<HibernateException>(() => t.TransformTuple(new object[] {1}, aliases));
		}
	}
}