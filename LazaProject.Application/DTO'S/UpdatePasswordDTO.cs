using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Core.DTO_S
{
    public class UpdatePasswordDTO
    {
		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Invalid email format.")]
		public string Email { get; set; }


		[Required(ErrorMessage = "New password is required.")]
		[StringLength(100, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
		public string NewPassword { get; set; }

		[Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}
}
