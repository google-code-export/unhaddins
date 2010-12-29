using NUnit.Framework;

namespace uNHAddins.Examples.Course.Tests.Inheritance
{
	[TestFixture]
	public class TablePerClassHierarchyFixture : InheritanceTestCase
	{
		protected override System.Collections.IList Mappings
		{
			get
			{
				return new string[] { "Inheritance.TablePerClassHierarchy.hbm.xml" };
			}
		}
	}
}
