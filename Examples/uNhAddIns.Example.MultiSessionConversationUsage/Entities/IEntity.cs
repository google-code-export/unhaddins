using System;

namespace uNhAddIns.Example.MultiSessionConversationUsage.Entities
{
	public interface IEntity: IEquatable<IEntity>
	{
		int Id { get; }
	}
}