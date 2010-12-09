using Castle.Components.Validator;

namespace uNhAddIns.CastleValidator.Tests
{
	public class Person
	{
		public const string NameErrorMessage = "Name is required.";
		public const string AgeErrorMessage = "age should be between 1 and 25";

		[ValidateNonEmpty(NameErrorMessage)]
		public string Name{ get; set;}

		[ValidateRange(1, 25, AgeErrorMessage)]
		public decimal Age { get; set; }
	}
}