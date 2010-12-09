using System.Windows;
using ChinookMediaManager.GUI.Shell;
using Microsoft.Practices.ServiceLocation;
using uNhAddIns.Adapters;

namespace ChinookMediaManager.GUI
{
	public partial class App
	{
		private readonly IGuyWire guyWire = ApplicationConfiguration.GetGuyWire();

		public App()
		{
			
		}
		
		protected override IServiceLocator CreateContainer()
		{
			guyWire.Wire();
			return ServiceLocator.Current;
		}

		protected override object CreateRootModel()
		{
			return Container.GetInstance<IShellViewModel>();
		}

		protected override void OnExit(ExitEventArgs e)
		{
			guyWire.Dewire();
			base.OnExit(e);
		}
	}
}