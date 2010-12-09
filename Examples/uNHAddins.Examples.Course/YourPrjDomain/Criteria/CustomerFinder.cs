using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Expressions;

namespace YourPrjDomain.Criteria
{
	public class CustomerFinder
	{
		#region Properties

		private bool? accountActive;
		private MoneyType? accountMoney;
		private DateTime? accountOpenedEnd;
		private DateTime? accountOpenedStart;
		private double? accountSizeMax;
		private double? accountSizeMin;

		private DateTime? born;
		private string city;
		private string customerId;
		private string firstName;
		private string lastName;
		private string zip;

		public DateTime? AccountOpenedStart
		{
			get { return accountOpenedStart; }
			set { accountOpenedStart = value; }
		}

		public DateTime? AccountOpenedEnd
		{
			get { return accountOpenedEnd; }
			set { accountOpenedEnd = value; }
		}

		public string Zip
		{
			get { return zip; }
			set { zip = value; }
		}

		public string City
		{
			get { return city; }
			set { city = value; }
		}

		public MoneyType? AccountMoney
		{
			get { return accountMoney; }
			set { accountMoney = value; }
		}

		public double? AccountSizeMax
		{
			get { return accountSizeMax; }
			set { accountSizeMax = value; }
		}

		public double? AccountSizeMin
		{
			get { return accountSizeMin; }
			set { accountSizeMin = value; }
		}

		public bool? AccountActive
		{
			get { return accountActive; }
			set { accountActive = value; }
		}

		public DateTime? Born
		{
			get { return born; }
			set { born = value; }
		}

		public string CustomerId
		{
			get { return customerId; }
			set { customerId = value; }
		}

		public string LastName
		{
			get { return lastName; }
			set { lastName = value; }
		}

		public string FirstName
		{
			get { return firstName; }
			set { firstName = value; }
		}

		#endregion

		public IList<Customer> Find(ISession session)
		{
			DetachedCriteria query = DetachedCriteria.For(typeof (Customer));

			AddCustomerQuery(query);

			AddAccountQuery(query);

			AddCityQuery(query);

			return query.GetExecutableCriteria(session).List<Customer>();
		}

		private void AddAccountQuery(DetachedCriteria query)
		{
			DetachedCriteria accountCriteria = null;

			if (AccountActive.HasValue)
			{
				accountCriteria = query.CreateCriteria("Accounts");
				accountCriteria.Add(Expression.Eq("IsActive", AccountActive));
			}

			if (AccountOpenedStart.HasValue)
			{
				accountCriteria = accountCriteria ?? query.CreateCriteria("Accounts");
				accountCriteria.Add(Expression.Ge("Opened", AccountOpenedStart));
			}

			if (AccountOpenedEnd.HasValue)
			{
				accountCriteria = accountCriteria ?? query.CreateCriteria("Accounts");
				accountCriteria.Add(Expression.Le("Opened", AccountOpenedEnd));
			}

			if (AccountSizeMin.HasValue)
			{
				accountCriteria = accountCriteria ?? query.CreateCriteria("Accounts");
				accountCriteria.Add(Expression.Ge("Size", AccountSizeMin));
			}

			if (AccountSizeMax.HasValue)
			{
				accountCriteria = accountCriteria ?? query.CreateCriteria("Accounts");
				accountCriteria.Add(Expression.Le("Size", AccountSizeMax));
			}

			if(AccountMoney.HasValue)
			{
				accountCriteria = accountCriteria ?? query.CreateCriteria("Accounts");
				accountCriteria.Add(Expression.Eq("AccountType", AccountMoney));
			}
		}

		public void AddCustomerQuery(DetachedCriteria query)
		{
			if (!string.IsNullOrEmpty(FirstName))
			{
				query.Add(Expression.Like("FirstName", FirstName, MatchMode.Anywhere));
			}

			if (!string.IsNullOrEmpty(LastName))
			{
				query.Add(Expression.Like("LastName", LastName, MatchMode.Anywhere));
			}

			if (!string.IsNullOrEmpty(CustomerId))
			{
				query.Add(Expression.Eq("CustomerId", CustomerId));
			}
		}

		public void AddCityQuery(DetachedCriteria query)
		{
			if (!string.IsNullOrEmpty(City))
			{
				query.Add(Expression.Eq("City.Name", City));
			}

			if (!string.IsNullOrEmpty(Zip))
			{
				query.Add(Expression.Eq("City.Zip", Zip));
			}
		}
	}
}