using System.Collections.Generic;
using System.Reflection;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.ModelBuilder.Inspectors;
using uNhAddIns.Adapters.Common;

namespace uNhAddIns.CastleAdapters.AutomaticConversationManagement
{
	public class PersistenceConversationalComponentInspector : MethodMetaInspector
	{
		private ReflectionConversationalMetaInfoStore metaStore;

		public override void ProcessModel(IKernel kernel, ComponentModel model)
		{
			if (metaStore == null)
			{
				metaStore = (ReflectionConversationalMetaInfoStore)kernel[typeof(IConversationalMetaInfoStore)];
			}

			if (!ConfigureBasedOnAttributes(model))
			{
				return;
			}
			Validate(model, metaStore);

			AddConversationInterceptorIfIsConversational(model, metaStore);
		}

		private static void AddConversationInterceptorIfIsConversational(ComponentModel model, IConversationalMetaInfoStore store)
		{
			var meta = store.GetMetadataFor(model.Implementation);

			if (meta == null)
			{
				return;
			}

			model.Dependencies.Add(new DependencyModel(DependencyType.Service, null, typeof (ConversationInterceptor), false));
			model.Interceptors.Add(new InterceptorReference(typeof (ConversationInterceptor)));
		}

		private static void Validate(ComponentModel model, IConversationalMetaInfoStore store)
		{
			if (model.Service == null || model.Service.IsInterface)
			{
				return;
			}

			var meta = store.GetMetadataFor(model.Implementation);

			if (meta == null)
			{
				return;
			}

			var problematicMethods = new List<string>(10);

			foreach (MethodInfo method in meta.Methods)
			{
				if (!method.IsVirtual)
				{
					problematicMethods.Add(method.Name);
				}
			}

			if (problematicMethods.Count != 0)
			{
				string[] methodNames = problematicMethods.ToArray();

				string message =
					string.Format(
						"The class {0} wants to use persistence-conversation interception, "
						+ "however the methods must be marked as virtual in order to do so. Please correct "
						+ "the following methods: {1}", model.Implementation.FullName, string.Join(", ", methodNames));

				throw new FacilityException(message);
			}
		}

		private bool ConfigureBasedOnAttributes(ComponentModel model)
		{
			return metaStore.Add(model.Implementation);
		}

		#region Overrides of MethodMetaInspector

		protected override string ObtainNodeName()
		{
			return "NotSupportedYet";
		}

		#endregion
	}
}