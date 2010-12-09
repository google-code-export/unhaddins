#region usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SessionManagement.Domain;
using SessionManagement.Domain.Model;
using SessionManagement.Presentation.Presenters;
using SessionManagement.Presentation.ViewInterfaces;

#endregion

namespace SessionManagement.GUI.Views
{
	public partial class BrowseProductsView : UserControl, IBrowseProductsView
	{
		#region Readonly & Static Fields

		#endregion

		#region Fields

		private BrowseProductsPresenter presenter;
		
		private EventHandler<TEventArgs<Product>> productSelected;
		public event EventHandler<TEventArgs<Product>> ProductSelected
		{
			add
			{
				productSelected += value;
				SelectionPanel.Visible = true;
			}
			remove
			{
				productSelected -= value;
				SelectionPanel.Visible = false;
			}
		}

		private void InvokeProductSelected(TEventArgs<Product> e)
		{
			var productSelectedHandler = productSelected;
			if (productSelectedHandler != null)
			{
				productSelectedHandler(this, e);
			}
		}

		#endregion

		#region C'tors

		public BrowseProductsView()
		{
			InitializeComponent();
		}

		#endregion

		#region Instance Methods

		private void InvokeViewInitialized(EventArgs e)
		{
			EventHandler viewInitializedHandler = ViewInitialized;
			if (viewInitializedHandler != null) viewInitializedHandler(this, e);
		}

		#endregion

		#region Event Handling

		private void BrowseProductsView_Load(object sender, EventArgs e)
		{
			if (!DesignMode)
			{
				presenter = new BrowseProductsPresenter(this);
				InvokeViewInitialized(EventArgs.Empty);
			}
		}

		#endregion

		#region IBrowseProductsView Members

		public void Clean()
		{
		}

		public void ShowMessage(string message)
		{
			MessageBox.Show(message);
		}

		public event EventHandler ViewInitialized;

		public event EventHandler CloseView;

		private void InvokeCloseView(EventArgs e)
		{
			EventHandler closeViewHandler = CloseView;
			if (closeViewHandler != null) closeViewHandler(this, e);
		}

		public Product SelectedProduct
		{
			get
			{
				if (dataGridView1.CurrentRow != null)
				{
					return productBindingSource[dataGridView1.CurrentRow.Index] as Product;
				}

				return null;
			}
		}

		public void SetProducts(IList<Product> products)
		{
			productBindingSource.DataSource = new BindingList<Product>(products);
		}

		#endregion

		private void button1_Click(object sender, EventArgs e)
		{
			SelectCurrentProduct();
		}

		private void SelectCurrentProduct()
		{
			if (SelectedProduct != null)
			{
				InvokeProductSelected(new TEventArgs<Product>(SelectedProduct));
				InvokeCloseView(EventArgs.Empty);
			}
		}

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			SelectCurrentProduct();
		}
	}
}