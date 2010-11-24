using System.Linq;
using NUnit.Framework;

namespace uNhAddIns.PostSharpAdapters.Tests
{

	public class Foo
	{
		public void Method1(){}
	}

	public interface IFoo
	{
		void Method1();
	}

	public class Bar : IFoo
	{
		public void Method1() { }
	}

	public class BarBar : Bar
	{
		
	}

	public class Bar2
	{
		[PersistenceConversation]
		private void DoSomething(){}

		[PersistenceConversation(Exclude = true)]
		public void DoSomethingElse(){}

		
	}

	public class Baz
	{
		[PersistenceConversation(Exclude = true)]
		public string SomeProperty { get; set; }
		
		[PersistenceConversation]
		private string SomePrivatePropertyIncluded { get; set; }
	}


	[TestFixture]
	public class ConversationalMethodInspectorFixture
	{
		[Test]
		public void should_work_with_simple_method()
		{
			var aspect = new ConversationalMethodInspector(
				typeof (Foo), 
				new PersistenceConversationalAttribute());
			aspect.GetMethods().Count().Should().Be.EqualTo(1);
		}

		[Test]
		public void should_return_interface_method_implementation()
		{
			var aspect = new ConversationalMethodInspector(
				typeof(Bar),
				new PersistenceConversationalAttribute());
			aspect.GetMethods().Count().Should().Be.EqualTo(1);
		}

		[Test]
		public void should_return_private_methods_that_contains_persistence_conversation_attribute()
		{
			var aspect = new ConversationalMethodInspector(
				typeof(Bar2),
				new PersistenceConversationalAttribute());
			aspect.GetMethods().First().Name.Should().Be.EqualTo("DoSomething");
		}

		[Test]
		public void should_exclude_public_methods_that_are_excluded()
		{
			var aspect = new ConversationalMethodInspector(
				typeof(Bar2),
				new PersistenceConversationalAttribute());
			aspect.GetMethods().Any(m => m.Name == "DoSomethingElse").Should().Be.False();
		}
               

		[Test]
		public void should_exclude_public_methods_when_the_aspect_is_excplit()
		{
			var aspect = new ConversationalMethodInspector(
				typeof(Foo),
				new PersistenceConversationalAttribute{MethodsIncludeMode = MethodsIncludeMode.Explicit});
			aspect.GetMethods().Should().Be.Empty();
		}

		[Test]
		public void getmethods_doesnt_retrieve_inherited_methods()
		{
			var aspect = new ConversationalMethodInspector(
				typeof(BarBar),
				new PersistenceConversationalAttribute());
			aspect.GetMethods().Count().Should().Be.EqualTo(0);
		}

		[Test]
		public void exclude_at_property_level_works()
		{
			var methodInspector = new ConversationalMethodInspector(typeof (Baz), new PersistenceConversationalAttribute());
			methodInspector.GetMethods().Select(m => m.Name).Should().Not.Contain("set_SomeProperty");
		}

		[Test]
		public void include_private_property_should_work()
		{
			var methodInspector = new ConversationalMethodInspector(typeof(Baz), new PersistenceConversationalAttribute());
			methodInspector.GetMethods().Select(m => m.Name)
				.Should().Contain("set_SomePrivatePropertyIncluded");
		}
	}
}