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
		[JsonIgnore]
		public string? UserId { get; set; }
		public string Id { get; set; }
		public string? Name { get; set; }
		public decimal? Price { get; set; }
		public string? Img { get; set; }

	}
}
