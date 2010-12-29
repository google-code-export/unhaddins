using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
//using log4net.Config;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using uNhAddIns.CastleAdapters;
using uNhAddIns.CastleAdapters.AutomaticConversationManagement;
using uNhAddIns.Example.AspNetMVCConversationUsage.Controllers;
using uNhAddIns.Example.AspNetMVCConversationUsage.DatabaseAccessObjects;
using uNhAddIns.Example.AspNetMVCConversationUsage.Utils;
using uNhAddIns.SessionEasier;
using uNhAddIns.SessionEasier.Conversations;
using MvcContrib.Castle;


namespace uNhAddIns.Example.AspNetMVCConversationUsage
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        static IWindsorContainer container;
        
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                        // URL with parameters
                new { controller = "Products", action = "Index", id = "" }  // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
            container = new WindsorContainer();
            
            InitializeNHSessionManagement();
            RegisterWebComponents();
            RegisterComponents();
            InitializeServiceLocator();

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));
        }

        protected static void InitializeServiceLocator()
        {
            var sl = new WindsorServiceLocator(container);
            container.Register(Component.For<IServiceLocator>().Instance(sl));
            ServiceLocator.SetLocatorProvider(() => sl);
        }

        static void RegisterWebComponents()
        {
            container.Register(
                AllTypes.Of<Controller>().
                    FromAssembly(typeof(ProductsController).Assembly)
                    .Configure(reg => reg.Named(reg.ServiceType.Name).LifeStyle.Transient)
            );
        }

        static void RegisterComponents()
        {
            container.Register(
                Component.For<IProductDao>().ImplementedBy<ProductDao>(),
                Component.For<IProductCategoryService>().ImplementedBy<ProductCategoryService>()
                );
            container.AddComponent("generic.dao", typeof(IDao<>), typeof(BaseDao<>));
            container.AddComponent("sampler", typeof(ISampler), typeof(Sampler));
        }

        protected void Application_End(object sender, EventArgs e)
        {
            container.Dispose();
        }

        static void InitializeNHSessionManagement()
        {
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

        public IWindsorContainer Container
        {
            get { return container; }
        }
    }
}