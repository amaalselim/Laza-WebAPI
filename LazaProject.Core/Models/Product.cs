using LazaProject.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LazaProject.Core.Models
{
	public class Product
	{
		[Key]
		public string Id { get; set; } = Guid.NewGuid().ToString();
		[Required]
		public string Name { get; set; }
		public string Description { get; set; }
		public string Img {  get; set; }	
		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }
		[ForeignKey("Category")]
		public string CategoryId { get; set; }
		public Category? Category { get; set; }
		public ICollection<productImage> Images { get; set; } = new List<productImage>(); 
		public ICollection<Reviews> Reviews { get; set; }= new List<Reviews>();	
	}
}
