using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Email is Required")]
		[EmailAddress(ErrorMessage = "Invaild Email")]
		public string Email  { get; set; }


		[Required(ErrorMessage = "Password is Required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required(ErrorMessage = "Dont Need The RemenberYou?")]
		public bool RemenberMe { get; set; }
	}
}
