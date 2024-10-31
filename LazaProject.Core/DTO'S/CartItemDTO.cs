using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Core.DTO_S
{
	public class CartItemDTO
	{
		public string ProductId { get; set; }
		public string? ProductName { get; set; }
		public string? ProductImg {  get; set; }
		public decimal? Price { get; set; }
		public int Quantity { get; set; }

	}
}
