using System;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using Spring.Context;
using uNhAddIns.Adapters.CommonTests;
using uNhAddIns.Adapters.CommonTests.Integration;
using Spring.Context.Support;
using uNhAddIns.SessionEasier;

namespace uNhAddIns.SpringAdapters.Tests.ConversationManagement
{
    [TestFixture]
    public class FullCreamFixture : FullCreamFixtureBase
    {
        #region Overrides of FullCreamFixtureBase

        protected override void InitializeServiceLocator()
        {
            var context =
                (IConfigurableApplicationContext)ContextRegistry.GetContext();
            var objectFactory = context.ObjectFactory;
            // TODO: Make it work trough XML
            objectFactory.RegisterDefaultConversationAop();

            //Set Fixtures' ServiceLocator
            var sl = new SpringServiceLocatorAdapter(objectFactory);
            objectFactory.RegisterInstance<IServiceLocator>(sl);
            ServiceLocator.SetLocatorProvider(() => sl);

            //Register SillyCrudModel dependant on DaoFactory (which uses the ServiceLocator)
            objectFactory.Register<IDaoFactory, DaoFactory>();
            objectFactory.RegisterPrototype<ISillyCrudModel, SillyCrudModel>();
			objectFactory.RegisterPrototype<ISillyReportModel, SillyReportModel>();
        }

        #endregion
    }
}