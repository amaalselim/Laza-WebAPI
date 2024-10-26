using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Core.DTO_S
{
	public class ForgetPasswordDTO
	{
		[EmailAddress(ErrorMessage = "Invalid email format.")]
		public string Email { get; set; }	
	}
}
