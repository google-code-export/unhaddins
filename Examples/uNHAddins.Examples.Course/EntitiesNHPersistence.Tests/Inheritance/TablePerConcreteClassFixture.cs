using NUnit.Framework;

namespace uNHAddins.Examples.Course.Tests.Inheritance
{
	[TestFixture]
	public class TablePerConcreteClassFixture : InheritanceTestCase
	{
		protected override System.Collections.IList Mappings
		{
			get
			{
				return new string[] { "Inheritance.TablePerConcreteClass.hbm.xml" };
			}
		}
	}
}
