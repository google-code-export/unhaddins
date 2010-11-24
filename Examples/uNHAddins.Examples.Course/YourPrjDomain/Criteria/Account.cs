using System;

namespace YourPrjDomain.Criteria
{
	public enum MoneyType
	{
		Dollar,
		Euro
	}

	public class Account : IEquatable<Account>
	{
		private int accountNumber;
		private MoneyType accountType;
		private int id;

		private bool isActive;
		private DateTime opened;
		private double size;

		public Account(int accountNumber, MoneyType accountType, bool isActive, DateTime opened, double size)
		{
			this.accountNumber = accountNumber;
			this.accountType = accountType;
			this.isActive = isActive;
			this.opened = opened;
			this.size = size;
		}

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		public int AccountNumber
		{
			get { return accountNumber; }
			set { accountNumber = value; }
		}

		public MoneyType AccountType
		{
			get { return accountType; }
			set { accountType = value; }
		}

		public bool IsActive
		{
			get { return isActive; }
			set { isActive = value; }
		}

		public DateTime Opened
		{
			get { return opened; }
			set { opened = value; }
		}

		public double Size
		{
			get { return size; }
			set { size = value; }
		}

		#region Equals and GetHashCode

		public Account()
		{
		}

		public bool Equals(Account account)
		{
			if (account == null) return false;
			return accountNumber == account.accountNumber;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj)) return true;
			return Equals(obj as Account);
		}

		public override int GetHashCode()
		{
			return accountNumber;
		}

		#endregion
	}
}