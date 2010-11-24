using SessionManagement.Domain;
using SessionManagement.Presentation.ViewInterfaces;

namespace SessionManagement.Presentation.Presenters
{
	public abstract class Presenter<TView> where TView : IView
	{
		private TView view;
		protected virtual TView View
		{
			get { return view; }
			set { view = value; }
		}

		protected Presenter(TView view)
		{
			this.view = view;
			this.view.ViewInitialized += ViewInitialized;
		}

		protected abstract void ViewInitialized(object sender, System.EventArgs e);
	}
}