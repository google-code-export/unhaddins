using System;
using Iesi.Collections.Generic;

namespace YourPrjDomain.Criteria
{
	public class Customer
	{
		private ISet<Account> accounts = new HashedSet<Account>();
		private DateTime born;
		private City city;
		private string customerId;
		private string firstName;
		private int id;
		private string lastName;

		private int quantityOfAccounts;

		public int QuantityOfAccounts
		{
			get { return quantityOfAccounts; }
			set { quantityOfAccounts = value; }
		}


		//Id for business issues
		public string CustomerId
		{
			get { return customerId; }
			set { customerId = value; }
		}

		//Id for persistence issues
		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		public string FirstName
		{
			get { return firstName; }
			set { firstName = value; }
		}

		public string LastName
		{
			get { return lastName; }
			set { lastName = value; }
		}

		public DateTime Born
		{
			get { return born; }
			set { born = value; }
		}

		public City City
		{
			get { return city; }
			set { city = value; }
		}

		public ISet<Account> Accounts
		{
			get { return accounts; }
			set { accounts = value; }
		}

		public void AddAccount(Account _account)
		{
			if (_account == null) throw new ArgumentNullException("_account");
			if (!this.Accounts.Add(_account))
				throw new InvalidOperationException("The account already exists");
		}

		public void RemoveAccount(Account _account)
		{
			if (_account == null) throw new ArgumentNullException("_account");

			this.Accounts.Remove(_account);
		}
	}
}