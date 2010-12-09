using System;

namespace uNhAddIns.Example.AopConversationUsage.Entities
{
	public interface IEntity: IEquatable<IEntity>
	{
		int Id { get; }
	}
}