using System;

namespace uNhAddIns.Entities
{
	public interface IGenericEntity<TIdentity> : IEquatable<IGenericEntity<TIdentity>>
	{
		TIdentity Id { get; }
	}
}