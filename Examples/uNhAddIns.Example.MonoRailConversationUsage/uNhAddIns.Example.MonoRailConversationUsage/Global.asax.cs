using System;
using System.IO;
using System.Reflection;
using System.Web;
using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using Castle.MonoRail.Framework;
using Castle.MonoRail.Framework.Configuration;
using Castle.MonoRail.Framework.Internal;
using Castle.MonoRail.Views.Brail;
using Castle.MonoRail.WindsorExtension;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using uNhAddIns.CastleAdapters.AutomaticConversationManagement;
using uNhAddIns.Example.MonoRailConversationUsage.Controllers;
using uNhAddIns.Example.MonoRailConversationUsage.DatabaseAccessObjects;
using uNhAddIns.Example.MonoRailConversationUsage.Utils;
using uNhAddIns.LinFuAdapters;
using uNhAddIns.SessionEasier;
using uNhAddIns.SessionEasier.Conversations;
using IFilter=Castle.MonoRail.Framework.IFilter;

namespace uNhAddIns.Example.MonoRailConversationUsage {
    public class Global : HttpApplication, IMonoRailConfigurationEvents, IContainerAccessor {
        static IWindsorContainer container;

        protected void Application_Start(object sender, EventArgs e) {
            ApplicationConfigure();
        }

        static void ApplicationConfigure() {
            container = new WindsorContainer();
            container.AddFacility("monorail", new MonoRailFacility());
            container.AddFacility("logging", new LoggingFacility(LoggerImplementation.Log4net, "log4net.config"));
            InitializeNHSessionManagement();
            RegisterWebComponents();
            RegisterComponents();
            InitializeServiceLocator();
        }

        static void InitializeNHSessionManagement() {
            container.AddFacility<PersistenceConversationFacility>();
            var nhConfigurator = new DefaultSessionFactoryConfigurationProvider();
            nhConfigurator.AfterConfigure += ((sender, e) => new SchemaExport(e.Configuration).Create(false, true));
            var sfp = new SessionFactoryProvider(nhConfigurator);
            container.Register(Component.For<ISessionFactoryProvider>().Instance(sfp));
            // I prefer to use uNhAddIns.LinFuAdapters.SessionWrapper , the discussion: http://groups.google.com/group/unhaddins/browse_thread/thread/e548a6f6a8a444fd
            container.Register(Component.For<ISessionWrapper>().ImplementedBy<SessionWrapper>());
            container.Register(Component.For<ISessionFactory>().Instance(sfp.GetFactory(null)));
            container.Register(Component.For<IConversationFactory>().ImplementedBy<DefaultConversationFactory>());
            container.Register(Component.For<IConversationsContainerAccessor>().ImplementedBy<NhConversationsContainerAccessor>());
        }

        protected static void InitializeServiceLocator() {
            var sl = new WindsorServiceLocator(container);
            container.Register(Component.For<IServiceLocator>().Instance(sl));
            ServiceLocator.SetLocatorProvider(() => sl);
        }

        static void RegisterWebComponents() {
            container.Register(
                AllTypes.Of<SmartDispatcherController>().
                    FromAssembly(typeof(TestController).Assembly),
                AllTypes.Of<IFilter>().
                    FromAssembly(typeof(TestController).Assembly),
                AllTypes.Of<ViewComponent>().FromAssembly(typeof(Global).Assembly)
                    .Configure(reg => reg.Named(reg.ServiceType.Name))
            );
        }

        static void RegisterComponents() {
            container.Register(
                Component.For<IProductDao>().ImplementedBy<ProductDao>(),
                Component.For<IProductCategoryService>().ImplementedBy<ProductCategoryService>()
                );
            container.AddComponent("generic.dao", typeof (IDao<>), typeof (BaseDao<>));
            container.AddComponent("sampler", typeof (ISampler), typeof (Sampler));
        }

        protected void Application_End(object sender, EventArgs e) {
            container.Dispose();
        }

        public void Configure(IMonoRailConfiguration configuration) {
            configuration.ControllersConfig.AddAssembly(Assembly.GetExecutingAssembly());
            configuration.ViewEngineConfig.ViewPathRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views");
            configuration.ViewEngineConfig.ViewEngines.Add(new ViewEngineInfo(typeof(BooViewEngine), false));
        }

        public IWindsorContainer Container {
            get { return container; }
        }
    }
}