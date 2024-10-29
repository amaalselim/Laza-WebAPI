using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LazaProject.Core.DTO_S
{
	public class WishListItemDTO
	{
		public string UserId { get; set; }
		public string ProductId { get; set; }
		public string? ProductName { get; set; }
		public decimal? ProductPrice { get; set; }

	}
}
