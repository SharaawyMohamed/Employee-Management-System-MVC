using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "Password Is Required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password Is Required")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Confirm Password Doensn't Match with Password")]
		public string ConfirmPassword { get; set; }
	}
}
