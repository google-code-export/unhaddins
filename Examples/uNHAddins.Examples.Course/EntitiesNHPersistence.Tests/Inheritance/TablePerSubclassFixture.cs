using NUnit.Framework;
namespace uNHAddins.Examples.Course.Tests.Inheritance
{
	[TestFixture]
	public class TablePerSubclassFixture : InheritanceTestCase
	{
		protected override System.Collections.IList Mappings
		{
			get
			{
				return new string[] { "Inheritance.TablePerSubclass.hbm.xml" };
			}
		}
	}
}
