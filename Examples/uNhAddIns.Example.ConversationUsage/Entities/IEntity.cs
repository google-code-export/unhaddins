using System;

namespace uNhAddIns.Example.ConversationUsage.Entities
{
	public interface IEntity: IEquatable<IEntity>
	{
		int Id { get; }
	}
}