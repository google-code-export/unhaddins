using System;

namespace uNhAddIns.PostSharpAdapters.Tests.Model
{
	public class Product
	{ }

	[PersistenceConversational]
	public class SamplePresenter
	{
		public Product GetProduct(Guid id)
		{
			return new Product();
		}

		[PersistenceConversation(ConversationEndMode = EndMode.CommitAndContinue)]
		public void DeleteInmediatly(Product product)
		{ }

		[PersistenceConversation(ConversationEndMode = EndMode.End)]
		public void AcceptAll()
		{ }

		[PersistenceConversation(Exclude = true)]
		public void DoSomethingNoPersistent()
		{
			DoSomethingNoPersistentPrivate();
		}

		private void DoSomethingNoPersistentPrivate(){}

		[PersistenceConversation(Exclude = true)]
		public string PropertyOutConversation { get { return "Foo"; } }

		public string PropertyInConversation { get { return "Baz"; } }

		[PersistenceConversation]
		private Product GetProductInternal()
		{
			return new Product();
		}

		public Product GetProductWithNestedMethod()
		{
			return GetProductInternal();
		}
	}

	[PersistenceConversational(DefaultEndMode = EndMode.End)]
	public class SamplePresenterWithDefaultModeEnd
	{
		public Product GetProduct(Guid id)
		{
			return new Product();
		}
	}

	[PersistenceConversational(ConversationCreationInterceptor = typeof(FixedLogInterceptor))]
	public class SamplePresenterWithConcreteInterception
	{
		public Product GetProduct(Guid id)
		{
			return new Product();
		}
	}

	[PersistenceConversational(ConversationCreationInterceptor = typeof(ILogInterceptor))]
	public class SamplePresenterWithServiceInterception
	{
		public Product GetProduct(Guid id)
		{
			return new Product();
		}
	}

	
}