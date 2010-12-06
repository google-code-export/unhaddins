namespace uNhAddIns.Example.MultiSessionConversationUsage.Entities
{
	public class Person : BaseEntity
	{
		public virtual string Name { get; set; }
		public virtual int Document { get; set; }

		public override string ToString()
		{
			return string.Format("Name: {0}, Document: {1}", Name, Document);
		}
	}
}