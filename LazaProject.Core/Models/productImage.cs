using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace LazaProject.Core.Models
{
    public class productImage
    {
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		[Required]
		public string Image { get; set; }

		[ForeignKey("Product")]
		public string ProductId { get; set; }
		[NotMapped]
		public string ProductName { get; set; }
		[JsonIgnore]
		public Product? Product { get; set; }
	}
}
