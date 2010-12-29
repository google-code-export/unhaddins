namespace SessionManagement.Domain
{
	public class Product : BaseEntity
	{
		public virtual string Code { get; set; }
		public virtual string Description { get; set; }
		public virtual double Price { get; set; }
	}
}