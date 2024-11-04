using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LazaProject.Core.DTO_S
{
	public class AddressDTO
	{
		[JsonIgnore]
		public string? UserId { get; set; }
		public string UserName { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		[Phone(ErrorMessage = "Invalid phone number format.")]
		public string Phone { get; set; }
	}
}
