using LazaProject.Core.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
		public int? VerificationCode { get; set; }

	}
}
