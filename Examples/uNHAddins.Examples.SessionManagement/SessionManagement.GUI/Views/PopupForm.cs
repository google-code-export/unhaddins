using System;
using System.Windows.Forms;
using SessionManagement.Presentation.ViewInterfaces;

namespace SessionManagement.GUI.Views
{
	public partial class PopupForm : Form
	{
		private readonly Control view;

		public PopupForm()
		{
			InitializeComponent();
		}

		public PopupForm(Control view) : this()
		{
			var viewable = view as IView;
			if (viewable == null)
			{
				throw new Exception("The view does not implement the IView interface as expected");
			}

			this.view = view;
			ClientSize = this.view.Size;
			this.view.Dock = DockStyle.Fill;
			viewable.CloseView += viewable_CloseView;
			Controls.Add(this.view);
		}

		void viewable_CloseView(object sender, EventArgs e)
		{
			Close();
		}
	}
}
