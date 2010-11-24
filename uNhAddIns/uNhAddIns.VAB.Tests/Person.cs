using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace uNhAddIns.VAB.Tests
{
	public class Person
	{
		public const string NameErrorMessage = "Name is required.";
		public const string AgeErrorMessage = "age should be between 1 and 25";

		[NotNullValidator(MessageTemplate = NameErrorMessage)]
		public string Name{ get; set;}

		[RangeValidator(1, RangeBoundaryType.Inclusive, 
						25, RangeBoundaryType.Inclusive, 
						MessageTemplate = AgeErrorMessage)]
		public int Age { get; set; }
	}
}