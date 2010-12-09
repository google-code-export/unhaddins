using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;
using PostSharp.Reflection;
using uNhAddIns.Extensions;
using uNhAddIns.SessionEasier.Conversations;

namespace uNhAddIns.PostSharpAdapters
{

	/// <summary>
	/// Indicates that a class is involved in a persistentes conversation.
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
	[MulticastAttributeUsage(
			MulticastTargets.Class, 
			TargetMemberAttributes = MulticastAttributes.Public,
			AllowMultiple = false,
			PersistMetaData = true)]
	public class PersistenceConversationalAttribute : InstanceLevelAspect//, IConversationalIdHolder
	{
		protected static readonly object NestedMethodMarker = new object();
		protected static readonly Type BaseConventionType = typeof (IConversationCreationInterceptorConvention<>);

		protected static readonly ConversationPausedWatcher ConversationPausedWatcher
			= new ConversationPausedWatcher();

		private string conversationId;

		/// <summary>
		/// Initializes a new instance of the <see cref="PersistenceConversationalAttribute"/> class.
		/// </summary>
		public PersistenceConversationalAttribute()
		{
			DefaultEndMode = EndMode.Continue;
			UseConversationCreationInterceptorConvention = true;
			MethodsIncludeMode = MethodsIncludeMode.Implicit;
		}

		/// <summary>
		/// Fixed Conversation's Id for the target class.
		/// </summary>
		/// <remarks>
		/// Optional.
		/// <para>
		/// Only use it when multiple instances of the target class must work in the same conversation.
		/// </para>
		/// </remarks>
		public string ConversationId { get; set; }

		/// <summary>
		/// Conversation's Id prefix.
		/// </summary>
		/// <remarks>
		/// Optional.
		/// <para>
		/// The result conversation's Id will be composed by IdPrefix + UniqueId
		/// </para>
		/// </remarks>
		public string IdPrefix { get; set; }

		/// <summary>
		/// Define the way each method, of the target class, will be included in a persistent conversation.
		/// </summary>
		/// <remarks>
		/// Optional, default <see cref="uNhAddIns.Adapters.MethodsIncludeMode.Implicit"/>
		/// </remarks>
		public MethodsIncludeMode MethodsIncludeMode { get; set; }

		/// <summary>
		/// Define the <see cref="EndMode"/> of each method where not explicity declared.
		/// </summary>
		/// <remarks>
		/// Optional, default <see cref="EndMode.Continue"/>
		/// </remarks>
		public EndMode DefaultEndMode { get; set; }

		///<summary>
		/// Define the class where conversation's events handlers are implemented.
		///</summary>
		/// <remarks>
		/// The class must implements IConversationCreationInterceptor.
		/// </remarks>
		public Type ConversationCreationInterceptor { get; set; }

		/// <summary>
		/// Use the IoC container to discover the implementor of IConversationCreationInterceptorConvention{T}
		/// where T is the class indicated by <seealso cref="PersistenceConversationalAttribute"/>.
		/// </summary>
		public bool UseConversationCreationInterceptorConvention { get; set; }

		/// <summary>
		/// Allow persistent call outside of the service scope.	Usefull in combination with linq queries.
		/// </summary>
		/// <remarks>
		/// Optional, default false
		/// </remarks>
		public bool AllowOutsidePersistentCall { get; set; }


		protected IConversationsContainerAccessor ConversationsContainerAccessor
		{
			get { return ServiceLocator.Current.GetInstance<IConversationsContainerAccessor>(); }
		}

		protected IConversationFactory ConversationFactory
		{
			get { return ServiceLocator.Current.GetInstance<IConversationFactory>(); }
		}

		private static bool IsNoopConversationalMarkerActive
		{
			get
			{
				NoopConversationalMarker noopServiceMarker = ServiceLocator
					.Current
					.GetAllInstances<NoopConversationalMarker>().FirstOrDefault();

				return noopServiceMarker != null && noopServiceMarker.Noop;
			}
		}

		#region IConversationalIdHolder Members
		//[ImportMember("GetConversationId", IsRequired = false, Order = ImportMemberOrder.AfterIntroductions)]
		//public Func<string> GetConversationIdMethod;

		//[IntroduceMember(OverrideAction = MemberOverrideAction.Ignore, Visibility = Visibility.Family, IsVirtual = true)]
		public string GetConversationId()
		{
			if (string.IsNullOrEmpty(conversationId))
			{
				if (!string.IsNullOrEmpty(ConversationId))
				{
					conversationId = ConversationId;
				}
				else if (!string.IsNullOrEmpty(IdPrefix))
				{
					conversationId = IdPrefix + Guid.NewGuid();
				}
				else
				{
					conversationId = Guid.NewGuid().ToString();
				}
			}
			return conversationId;
		}

		#endregion

		[AspectTypeDependency(AspectDependencyAction.Commute, typeof(PersistenceConversationalAttribute))]
		[OnMethodExceptionAdvice(Master = "OnEntry")]
		public void OnException(MethodExecutionArgs eventArgs)
		{
			if (IsNoopConversationalMarkerActive) return;
			if (eventArgs.Exception is ConversationException)
			{
				ConversationsContainerAccessor.Container.Unbind(GetConversationId());
			}
			eventArgs.FlowBehavior = FlowBehavior.RethrowException;
		}

		[AspectTypeDependency(AspectDependencyAction.Commute, typeof(PersistenceConversationalAttribute))]
		[MethodPointcut("GetMethods")]
		[OnMethodEntryAdvice]
		public void OnEntry(MethodExecutionArgs eventArgs)
		{
			if (IsNoopConversationalMarkerActive) return;

			var convId = GetConversationId();
			IConversation c = ConversationsContainerAccessor.Container.Get(convId);
			if (c == null)
			{
				IConversationFactory cf = ConversationFactory;
				if (cf == null)
				{
					return;
				}
				c = cf.CreateConversation(convId);
				// we are using the event because a custom eventHandler can prevent the rethrow
				// but we must Unbind the conversation from the container
				// and we must dispose the conversation itself (high probability UoW inconsistence).
				c.OnException += ((conversation, args) => ConversationsContainerAccessor.Container.Unbind(c.Id).Dispose());
				ConfigureConversation(c, eventArgs.Instance);
				ConversationsContainerAccessor.Container.SetAsCurrent(c);
				c.Start();
				ConversationPausedWatcher.Watch(c);
			}
			else
			{
				ConversationsContainerAccessor.Container.SetAsCurrent(c);
				if (ConversationPausedWatcher.IsPaused(c))
				{
					c.Resume();
				}
				else
				{
					eventArgs.MethodExecutionTag = NestedMethodMarker;
				}
			}
		}

		private void ConfigureConversation(IConversation conversation, object instance)
		{
			IConversationCreationInterceptor cci = null;
			Type creationInterceptorType = ConversationCreationInterceptor;
			if (creationInterceptorType != null)
			{
				cci = creationInterceptorType.IsInterface
				      	? GetConversationCreationInterceptor(creationInterceptorType)
				      	: creationInterceptorType.Instantiate<IConversationCreationInterceptor>();
			}
			else
			{
				if (UseConversationCreationInterceptorConvention)
				{
					Type concreteImplementationType = BaseConventionType.MakeGenericType(instance.GetType());
					cci = GetConversationCreationInterceptor(concreteImplementationType);
				}
			}
			if (cci != null)
			{
				cci.Configure(conversation);
			}
			if (AllowOutsidePersistentCall)
			{
				var conversationWithOpc = conversation as ISupportOutsidePersistentCall;
				if (conversationWithOpc == null)
				{
					throw new InvalidOperationException("The conversation doesn't support outside persistent call");
				}
				conversationWithOpc.UseSupportForOutsidePersistentCall = true;
			}
		}

		private static IConversationCreationInterceptor GetConversationCreationInterceptor(Type creationInterceptorType)
		{
			return (IConversationCreationInterceptor) ServiceLocator
			                                          	.Current
			                                          	.GetAllInstances(creationInterceptorType)
			                                          	.FirstOrDefault();
		}

		public IEnumerable<MethodInfo> GetMethods(Type type)
		{
			var methodInspector = new ConversationalMethodInspector(type, this);
			return methodInspector.GetMethods();
		}

		public EndMode GetMethodEndMode(MethodBase methodInfo)
		{
			if (!methodInfo.IsDefined(typeof (PersistenceConversationAttribute), true))
			{
				return DefaultEndMode;
			}
			var attributeValue = methodInfo.GetCustomAttributes(typeof (PersistenceConversationAttribute), true)
				.OfType<PersistenceConversationAttribute>()
				.First().ConversationEndMode;

			return attributeValue == EndMode.Unspecified ? DefaultEndMode : attributeValue;
		}

		[AspectTypeDependency(AspectDependencyAction.Commute, typeof(PersistenceConversationalAttribute))]
		[OnMethodSuccessAdvice(Master = "OnEntry")]
		public void OnSuccess(MethodExecutionArgs eventArgs)
		{
			if (IsNoopConversationalMarkerActive) return;
			if (eventArgs.MethodExecutionTag == NestedMethodMarker) return;
			IConversationsContainerAccessor cca = ConversationsContainerAccessor;
			IConversation c = cca.Container.Get(GetConversationId());
			
			EndMode endMode = GetMethodEndMode(eventArgs.Method);
			switch (endMode)
			{
				case EndMode.End:
					c.End();
					c.Dispose();
					break;
				case EndMode.Abort:
					c.Abort();
					c.Dispose();
					break;
				case EndMode.CommitAndContinue:
					c.FlushAndPause();
					break;
				default:
					c.Pause();
					break;
			}
		}
	}
}