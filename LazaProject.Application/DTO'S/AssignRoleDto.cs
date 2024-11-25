using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LazaProject.Core.Enums;

namespace LazaProject.Core.DTO_S
{
    public class AssignRoleDto
	{
		[EmailAddress(ErrorMessage = "Invalid email format.")]
		public string Email { get; set; }
		public Roles Role { get; set; }
	}
}
