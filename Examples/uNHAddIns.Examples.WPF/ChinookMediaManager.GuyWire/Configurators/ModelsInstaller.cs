using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using ChinookMediaManager.Domain.Impl;

namespace ChinookMediaManager.GuyWire.Configurators
{
	public class ModelsInstaller : IWindsorInstaller
	{
		#region IWindsorInstaller Members

		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(AllTypes.FromAssemblyContaining<AlbumManager>()
										.Where(t => !t.IsAbstract && !t.IsInterface)
										.WithService.FirstInterface());
		}

		#endregion
	}
}