using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
//using NHibernate.ByteCode.Castle;
using NHibernate;
using NHibernate.ByteCode.Castle;
using NHibernate.Engine;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using uNhAddIns.CastleAdapters.EnhancedBytecodeProvider;
using uNhAddIns.NHibernateTypeResolver;
using Environment=NHibernate.Cfg.Environment;

namespace uNhAddIns.ComponentBehaviors.Castle.Tests
{
    public class IntegrationBaseTest
    {
        protected WindsorContainer container;
        protected NHibernate.Cfg.Configuration cfg;
        protected ISessionFactoryImplementor sessions;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            container = new WindsorContainer();
            log4net.Config.XmlConfigurator.Configure();

            ConfigureWindsorContainer();

            Environment.UseReflectionOptimizer = true;
            Environment.BytecodeProvider = new EnhancedBytecode(container);

            cfg = new NHibernate.Cfg.Configuration();

            cfg.Configure();
            
            cfg.Properties[Environment.ProxyFactoryFactoryClass] = GetProxyFactoryFactory();
			cfg.Properties[Environment.CurrentSessionContextClass] = "thread_static";
        	cfg.RegisterEntityNameResolver();
			cfg.Interceptor = GetInterceptor();
            cfg.AddAssembly(typeof(IntegrationBaseTest).Assembly);
            
            new SchemaExport(cfg).Create(true, true);
            sessions = (ISessionFactoryImplementor)cfg.BuildSessionFactory();


        	var componentInterceptor = cfg.Interceptor as Castle.NHibernateInterceptor.ComponentBehaviorInterceptor;
        	if(componentInterceptor  != null)
        		componentInterceptor.SessionFactory = sessions;
            
        }

    	protected virtual IInterceptor GetInterceptor()
    	{
    		return new EntityNameInterceptor();
    	}

    	protected virtual string GetProxyFactoryFactory()
        {
            return typeof (ProxyFactoryFactory).AssemblyQualifiedName;
        }

        protected virtual void ConfigureWindsorContainer()
        {}

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            new SchemaExport(cfg).Drop(false, true);
            sessions.Close();
            sessions = null;
            cfg = null;
        }
    }
}