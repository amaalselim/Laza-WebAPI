using LazaProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LazaProject.Core.DTO_S
{
	public class OrderConfirmationRequestDTO
	{
		[JsonIgnore]
		public string? UserId { get; set; }
		[JsonIgnore]
		public string? Email { get; set; }
		[JsonIgnore]

		public string? UserName { get; set; }
	}
}
