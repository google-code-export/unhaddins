using System;
namespace SessionManagement.Presentation.ViewInterfaces
{
	public interface IView
	{
		void Clean();
		void ShowMessage(string message);
		event EventHandler ViewInitialized;
		event EventHandler CloseView;
	}
}