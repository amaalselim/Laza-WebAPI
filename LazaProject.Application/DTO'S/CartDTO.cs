using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Core.DTO_S
{
	public class CartDTO
	{
		public string Id { get; set; }
		public string UserId { get; set; }
		public decimal TotalPrice { get; set; }
		public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
	}
}
