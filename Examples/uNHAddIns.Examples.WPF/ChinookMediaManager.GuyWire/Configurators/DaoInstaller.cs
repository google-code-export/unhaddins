using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using ChinookMediaManager.Data;
using ChinookMediaManager.Data.Impl;

namespace ChinookMediaManager.GuyWire.Configurators
{
	public class DaoInstaller : IWindsorInstaller
	{
		#region IWindsorInstaller Members

		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component.For(typeof (IDao<>))
										.Forward(typeof(IDaoReadOnly<>))
			                   			.ImplementedBy(typeof (Dao<>)));
		}

		#endregion
	}
}