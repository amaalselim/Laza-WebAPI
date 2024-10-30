using LazaProject.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Core.DTO_S
{
	public class RegisterDTO
	{
		public string Name { get; set; }
		[EmailAddress(ErrorMessage = "Invalid email format.")]
		public string Email { get; set; }
		public string Password { get; set; }
		public bool RememberMe { get; set; }
		public Gender Gender { get; set; }
	}
}
