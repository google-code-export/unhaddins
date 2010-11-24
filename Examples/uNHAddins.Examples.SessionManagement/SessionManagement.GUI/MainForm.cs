using System;
using System.Windows.Forms;
using SessionManagement.GUI.Views;
using SessionManagement.Infrastructure.InversionOfControl;
using SessionManagement.Data.SchemaCreation;

namespace SessionManagement.GUI
{
	public partial class MainForm : Form
	{
		private readonly ISchemaCreator schemaCreator;

		public MainForm()
		{
			InitializeComponent();
		}

		public MainForm(ISchemaCreator schemaCreator) : this()
		{
			this.schemaCreator = schemaCreator;
		}

		private void closeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void MenuItem_Click(object sender, EventArgs e)
		{
			var toolStripItem = sender as ToolStripItem;
			if (toolStripItem != null && toolStripItem.Tag != null)
			{
				var key = toolStripItem.Tag.ToString();
				ShowView(key);
			}
		}

		private void ShowView(string key)
		{
			using (var form = new PopupForm((Control) IoC.Resolve(key)))
			{
				form.ShowDialog();
			}
		}

		private void createToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			schemaCreator.CreateSchema();
		}

		private void dropToolStripMenuItem_Click(object sender, EventArgs e)
		{
			schemaCreator.DropSchema();
		}
	}
}
