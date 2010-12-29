using System;

namespace YourPrjDomain.Transformers
{
	public class Customer
	{
		private string address;
		private DateTime born;
		private string firstName;
		private int id;
		private string lastName;
		private string phone;
		private string zip;

		public string Address
		{
			get { return address; }
			set { address = value; }
		}

		public DateTime Born
		{
			get { return born; }
			set { born = value; }
		}

		public string FirstName
		{
			get { return firstName; }
			set { firstName = value; }
		}

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		public string LastName
		{
			get { return lastName; }
			set { lastName = value; }
		}

		public string Phone
		{
			get { return phone; }
			set { phone = value; }
		}

		public string Zip
		{
			get { return zip; }
			set { zip = value; }
		}
	}
}