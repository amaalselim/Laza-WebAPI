using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LazaProject.Core.Models
{
	public class WishListItem
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string UserId { get; set; }
		[ForeignKey("Product")]
		public string ProductId { get; set; }
		public virtual Product? Product { get; set; }
	}
}
