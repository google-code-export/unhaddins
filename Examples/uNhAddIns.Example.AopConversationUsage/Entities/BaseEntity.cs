namespace uNhAddIns.Example.AopConversationUsage.Entities
{
	public class BaseEntity : IEntity
	{
		// This implementation are supposing your are using "POID uniqueness across all classes" strategy.
		private int? requestedHashCode;

		#region IEntity Members

		public virtual int Id { get; private set; }

		public virtual bool Equals(IEntity other)
		{
			if (null == other)
			{
				return false;
			}

			return ReferenceEquals(this, other) || other.Id.Equals(Id);
		}

		#endregion

		protected virtual bool IsTransient()
		{
			return Equals(Id, 0);
		}

		public override bool Equals(object obj)
		{
			var that = obj as IEntity;
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