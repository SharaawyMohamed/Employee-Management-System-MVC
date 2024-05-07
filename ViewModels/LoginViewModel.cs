using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage ="Email Is Required")]
		[EmailAddress(ErrorMessage ="Invalid Email Address")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password Is Required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public bool Remember { get; set; }
	}
}
