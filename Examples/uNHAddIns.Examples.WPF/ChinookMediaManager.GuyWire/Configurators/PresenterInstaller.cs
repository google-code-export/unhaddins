using Caliburn.PresentationFramework.Screens;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using ChinookMediaManager.GUI.Shell;

namespace ChinookMediaManager.GuyWire.Configurators
{
	public class PresenterInstaller : IWindsorInstaller
	{
		#region IWindsorInstaller Members

		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(AllTypes.FromAssemblyContaining<ShellViewModel>()
			                   	.Where(t => !t.IsInterface && !t.IsAbstract && typeof (Screen).IsAssignableFrom(t) && !t.Equals(typeof (ShellViewModel)))
			                   	.Configure(c => c.LifeStyle.Transient));

			container.Register(Component.For<IShellViewModel>().ImplementedBy<ShellViewModel>());
		}

		#endregion
	}
}