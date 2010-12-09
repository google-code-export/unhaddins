using System;
using NHibernate;
using NUnit.Framework;
using uNhAddIns.Transform;

namespace uNhAddIns.Test.Transform
{
	[TestFixture]
	public class DelegateResultTransformer
	{
		private class ASimplePoco
		{
			public int IntProp { get; set; }

			public string StringProp { get; set; }
		}

		[Test]
		public void ConstructorInvalidType()
		{
			Assert.Throws<ArgumentNullException>(() => new DelegateTransformer<object>(null));
		}

	

		[Test]
		public void Setters()
		{
			var tuple = new object[] {1, "oreja"};

			var transformer = new DelegateTransformer<ASimplePoco>(t => new ASimplePoco
				                                          	{
				                                          		IntProp = (int) t[0], 
																StringProp = (string) t[1]
				                                          	});

			var poco = (ASimplePoco)transformer.TransformTuple(tuple, null);

			Assert.AreEqual(1, poco.IntProp); 
			Assert.AreEqual("oreja", poco.StringProp);
		}
	}
}