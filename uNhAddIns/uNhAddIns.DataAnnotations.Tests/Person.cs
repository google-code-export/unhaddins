using System.ComponentModel.DataAnnotations;

namespace uNhAddIns.DataAnnotations.Tests
{
	public class Person
	{
		public const string NameErrorMessage = "Name is required.";
		public const string AgeErrorMessage = "age should be between 1 and 25";

		[Required(ErrorMessage = NameErrorMessage)]
		public string Name{ get; set;}

		[Range(1, 25, ErrorMessage = AgeErrorMessage)]
		public decimal Age { get; set; }
	}
}