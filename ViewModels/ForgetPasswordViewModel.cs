using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class ForgetPasswordViewModel
	{
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }
	}
}
