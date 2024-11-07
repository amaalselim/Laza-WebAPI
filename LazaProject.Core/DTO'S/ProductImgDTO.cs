using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LazaProject.Core.DTO_S
{
    public class ProductImgDTO
    {
		public string Image { get; set; }
		[JsonIgnore]
		public string ProductId { get; set; }
		[JsonIgnore]
		public string ProductName { get; set; }
	}
}
