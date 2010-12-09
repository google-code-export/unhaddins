using ChinookMediaManager.Domain;
using NHibernate.Validator.Cfg.Loquacious;

namespace ChinookMediaManager.Data.Impl.Constraints
{
	public class AlbumValidationDef : ValidationDef<Album>
	{

		public AlbumValidationDef()
		{
			Define(x => x.Title)
				.NotEmpty().WithMessage("Title should not be null.")
				.And
				.MaxLength(200).WithMessage("Title should not exceed 200 chars.");
		}
		
	}
}