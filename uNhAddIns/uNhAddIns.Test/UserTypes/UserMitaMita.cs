using System.Collections.ObjectModel;

namespace uNhAddIns.Test.UserTypes
{
	public class UserMitaMita
	{
		public virtual string Name { get; set; }
		public virtual Country Country { get; set; }
	}

	public class Country
	{
		internal Country(int id)
		{
			Id = id;
		}

		public int Id { get; protected set; }

		public string Name { get; set; }
		public short PhonePrefix { get; set; }

		public bool Equals(Country other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return other.Id == Id;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Country);
		}

		public override int GetHashCode()
		{
			return Id;
		}
	}

	public class Countries : ReadOnlyCollection<Country>
	{
		public static Country Argentina = new Country(1) {Name = "Argentina", PhonePrefix = 54};
		public static Country Italy = new Country(2) {Name = "Italy", PhonePrefix = 39};
		public Countries() : base(new[] {Argentina, Italy}) {}
	}
}