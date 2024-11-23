using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LazaProject.Core.Models
{
	public class CartItem
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public int Quantity { get; set; }

		[ForeignKey("Cart")]
		public string CartId { get; set; }
		[JsonIgnore]
		public virtual Cart? Cart { get; set; }
		[ForeignKey("Product")]
		public string ProductId { get; set; }


		public decimal Price { get; set; }
		public virtual Product? Product { get; set; }
		public bool IsActive { get; set; } = true; 



	}
}
