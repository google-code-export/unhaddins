namespace SessionManagement.Domain
{
	public class BaseEntity 
	{
		private int? requestedHashCode;

		#region IEntity Members

		public virtual int Id { get; set; }

		public virtual bool Equals(BaseEntity other)
		{
			if (null == other || !GetType().IsInstanceOfType(other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}

			bool otherIsTransient = Equals(other.Id, 0);
			bool thisIsTransient = IsTransient();
			if (otherIsTransient && thisIsTransient)
			{
				return ReferenceEquals(other, this);
			}

			return other.Id.Equals(Id);
		}

		#endregion

		protected bool IsTransient()
		{
			return Equals(Id, 0);
		}

		public override bool Equals(object obj)
		{
			var that = obj as BaseEntity;
			return Equals(that);
		}

		public override int GetHashCode()
		{
			if (!requestedHashCode.HasValue)
			{
				requestedHashCode = IsTransient() ? base.GetHashCode() : Id.GetHashCode();
			}
			return requestedHashCode.Value;
		}
	}
}