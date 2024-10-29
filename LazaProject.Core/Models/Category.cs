using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LazaProject.Core.Models
{
	public class Category
	{
		public string Id { get; set; }= Guid.NewGuid().ToString();
		public string Name { get; set; }
		public string Img { get; set; }
		[JsonIgnore]
		public virtual ICollection<Product>? Products { get; set; }
	}
}
