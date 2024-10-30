using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Core.Models
{
	public class Reviews
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string UserId { get; set; }
		public string UserName { get; set; }
		public string Feedback { get; set; }
		[Range(0.0,5.0)]
		public decimal Rating { get; set; }
		[ForeignKey("Product")]
		public string productId { get; set; }
		public Product? Product { get; set; }
	}
}
