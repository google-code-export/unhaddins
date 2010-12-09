using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Engine;
using uNhAddIns.CastleAdapters;
using uNhAddIns.CastleAdapters.AutomaticConversationManagement;
using uNhAddIns.CastleAdapters.EnhancedBytecodeProvider;
using uNhAddIns.SessionEasier;
using uNhAddIns.SessionEasier.Conversations;
using ValidatorInitializer = NHibernate.Validator.Cfg.ValidatorInitializer;

namespace ChinookMediaManager.GuyWire.Configurators
{
	public class NHibernateInstaller : IWindsorInstaller
	{
		#region IWindsorInstaller Members

		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			
			Environment.BytecodeProvider = new EnhancedBytecode(container);


			var nhConfigurator = new DefaultSessionFactoryConfigurationProvider();

			nhConfigurator.BeforeConfigure += (sender, e) => ValidatorInitializer.Initialize(e.Configuration);

			var sfp = new SessionFactoryProvider(nhConfigurator);

			container.Register(Component.For<ISessionFactoryProvider>()
							   	.Instance(sfp));
			container.Register(Component.For<ISessionFactory>()
										.Forward<ISessionFactoryImplementor>()
										.UsingFactoryMethod(() => container.Resolve<ISessionFactoryProvider>().GetFactory(null)));

			container.Register(Component.For<ISessionWrapper>().ImplementedBy<SessionWrapper>());
			container.Register(Component.For<IConversationFactory>().ImplementedBy<DefaultConversationFactory>());
			container.Register(Component.For<IConversationsContainerAccessor>().ImplementedBy<NhConversationsContainerAccessor>());
		}

		#endregion
	}
}