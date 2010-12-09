namespace uNhAddIns.Entities
{
	public abstract class AbstractEntity<TIdentity> : IGenericEntity<TIdentity>
	{
		private int? requestedHashCode;

		#region IGenericEntity<TIdentity> Members

		public virtual TIdentity Id { get; protected set; }

		/// <summary>
		/// Compare equality trough Id
		/// </summary>
		/// <param name="other">Entity to compare.</param>
		/// <returns>true is are equals</returns>
		/// <remarks>
		/// Two entities are equals if they are of the same hierarcy tree/sub-tree
		/// and has same id.
		/// </remarks>
		public virtual bool Equals(IGenericEntity<TIdentity> other)
		{
			if (null == other || !GetType().IsInstanceOfType(other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}

			bool otherIsTransient = Equals(other.Id, default(TIdentity));
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
			return Equals(Id, default(TIdentity));
		}

		public override bool Equals(object obj)
		{
			var that = obj as IGenericEntity<TIdentity>;
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