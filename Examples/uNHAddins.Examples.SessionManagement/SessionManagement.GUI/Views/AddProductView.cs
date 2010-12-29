#region usings

using System;
using System.Windows.Forms;
using SessionManagement.Domain;
using SessionManagement.Domain.Model;
using SessionManagement.Presentation.Presenters;
using SessionManagement.Presentation.ViewInterfaces;

#endregion

namespace SessionManagement.GUI.Views
{
	public partial class AddProductView : UserControl, IAddProductView
	{
		#region Readonly & Static Fields

		#endregion

		#region Fields

		private AddProductPresenter presenter;

		#endregion

		#region C'tors

		public AddProductView()
		{
			InitializeComponent();
		}

		#endregion

		#region Instance Properties

		public string Code
		{
			get { return CodeTextBox.Text; }
		}

		public string Description
		{
			get { return DescriptionTextBox.Text; }
		}

		public double Price
		{
			get
			{
				double price;

				return double.TryParse(PriceTextBox.Text, out price)
				       	? price
				       	: 0.0;
			}
		}

		#endregion

		#region Instance Methods

		private Product GetProduct()
		{
			return new Product
			       	{
			       		Code = Code,
			       		Description = Description,
			       		Price = Price
			       	};
		}

		private void InvokeAddButtonPressed(TEventArgs<Product> e)
		{
			var addButtonPressedHandler = addButtonPressed;
			if (addButtonPressedHandler != null) addButtonPressedHandler(this, e);
		}

		private void InvokeViewInitialized(EventArgs e)
		{
			var viewInitializedHandler = viewInitialized;
			if (viewInitializedHandler != null) viewInitializedHandler(this, e);
		}

		#endregion

		#region Event Handling

		private void AddButton_Click(object sender, EventArgs e)
		{
			var product = GetProduct();
			InvokeAddButtonPressed(new TEventArgs<Product>(product));
		}

		private void AddProductView_Load(object sender, EventArgs e)
		{
			if (!DesignMode)
			{
				presenter = new AddProductPresenter(this);
				InvokeViewInitialized(EventArgs.Empty);
				CodeTextBox.Focus();
			}
		}

		private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsDigit(e.KeyChar) && !char.IsPunctuation(e.KeyChar) && !char.IsControl(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		#endregion

		#region Event Declarations

		private event EventHandler<TEventArgs<Product>> addButtonPressed;
		private event EventHandler viewInitialized;

		#endregion

		#region IAddProductView Members

		public event EventHandler<TEventArgs<Product>> AddButtonPressed
		{
			add { addButtonPressed += value; }
			remove { addButtonPressed -= value; }
		}

		public void Clean()
		{
			CodeTextBox.Text = string.Empty;
			DescriptionTextBox.Text = string.Empty;
			PriceTextBox.Text = string.Empty;
			CodeTextBox.Focus();
		}

		public void ShowMessage(string message)
		{
			MessageBox.Show(message);
		}

		public event EventHandler ViewInitialized
		{
			add { viewInitialized += value; }
			remove { viewInitialized -= value; }
		}

		public event EventHandler CloseView;

		#endregion
	}
}