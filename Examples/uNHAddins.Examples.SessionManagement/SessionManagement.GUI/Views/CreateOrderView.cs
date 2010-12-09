#region usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SessionManagement.Domain;
using SessionManagement.Domain.Model;
using SessionManagement.Presentation.Presenters;
using SessionManagement.Presentation.ViewInterfaces;
using SessionManagement.Infrastructure.InversionOfControl;

#endregion

namespace SessionManagement.GUI.Views
{
	public partial class CreateOrderView : UserControl, ICreateOrderView
	{
		#region Fields

		private CreateOrderPresenter presenter;

		#endregion

		#region C'tors

		public CreateOrderView()
		{
			InitializeComponent();
		}

		#endregion

		#region Instance Properties

		public string Number
		{
			get { return NumberTextBox.Text; }
		}

		public DateTime OrderDate
		{
			get { return dateTimePicker1.Value; }
		}

		public string OrderNumber
		{
			get { return NumberTextBox.Text; }
		}

		public IList<OrderLine> OrderLines
		{
			get
			{
				return orderLineBindingSource.DataSource as IList<OrderLine>;
			}
		}

		public void ShowLines(IList<OrderLine> lines)
		{
			orderLineBindingSource.DataSource = new BindingList<OrderLine>(lines);
			if (orderLineBindingSource.Count == 0)
			{
				orderLineBindingSource.AddNew();
			}

			dataGridView1.Focus();
		}

		#endregion

		#region Instance Methods

		private void InvokeAddButtonPressed(EventArgs e)
		{
			var addButtonPressedHandler = AddButtonPressed;
			if (addButtonPressedHandler != null) addButtonPressedHandler(this, e);
		}

		private void InvokeViewInitialized(EventArgs e)
		{
			var viewInitializedHandler = ViewInitialized;
			if (viewInitializedHandler != null) viewInitializedHandler(this, e);
		}

		#endregion

		#region Event Handling

		private void AddButton_Click(object sender, EventArgs e)
		{
			InvokeAddButtonPressed(EventArgs.Empty);			
		}

		private void AddProductView_Load(object sender, EventArgs e)
		{
			if (!DesignMode)
			{
				presenter = new CreateOrderPresenter(this);
				InvokeViewInitialized(EventArgs.Empty);
			}
		}

		#endregion

		#region Event Declarations

		public event EventHandler AddButtonPressed;
		public event EventHandler SaveButtonPressed;
		public event EventHandler ViewInitialized;
		public event EventHandler CloseView;

		#endregion

		#region ICreateOrderView Members

		private void InvokeSaveButtonPressed(EventArgs e)
		{
			var saveButtonPressedHandler = SaveButtonPressed;
			if (saveButtonPressedHandler != null)
			{
				saveButtonPressedHandler(this, e);
			}
		}

		public void Clean()
		{
			NumberTextBox.Text = string.Empty;
			NumberTextBox.Focus();
		}

		public void ShowMessage(string message)
		{
			MessageBox.Show(message);
		}

		#endregion

		private void SaveButton_Click(object sender, EventArgs e)
		{
			InvokeSaveButtonPressed(EventArgs.Empty);
		}

		private void browseProductsView_ProductSelected(object sender, TEventArgs<Product> e)
		{
			var line = orderLineBindingSource.Current as OrderLine;
			if (line != null && line.Product != e.Data)
			{
				line.Product = e.Data;
			}
		}

		private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0) return;

			if (e.ColumnIndex == ProductCodeColumn.DisplayIndex)
			{
				var browseProductsView = IoC.Resolve<IBrowseProductsView>();
				browseProductsView.ProductSelected += browseProductsView_ProductSelected;

				using (var form = new PopupForm((Control)browseProductsView))
				{
					form.ShowDialog();
				}
			}
		}
	}
}