using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LazaProject.Core.DTO_S
{
	public class CardDTO
	{
		[Required]
		[StringLength(100, ErrorMessage = "Card owner name should not exceed 100 characters.")]
		public string CardOwner { get; set; }

		[Required]
		[RegularExpression(@"^\d{4} \d{4} \d{4} \d{4}$", ErrorMessage = "Card number must be in format XXXX XXXX XXXX XXXX")]
		public string CardNumber { get; set; }

		[Required]
		[RegularExpression(@"^(0[1-9]|1[0-2])/[0-9]{2}$", ErrorMessage = "Expiration date must be in MM/YY format")]
		public string ExpirationDate { get; set; }

		[Required]
		[RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV must be 3 or 4 digits")]
		public string CVV { get; set; }
		[JsonIgnore]
		public string? UserId { get; set; }
	}
}
