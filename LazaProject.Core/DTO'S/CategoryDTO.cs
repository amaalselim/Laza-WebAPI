using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Core.DTO_S
{
    public class CategoryDTO
    {
		[Required]
		public string Name { get; set; }

		public IFormFile Img { get; set; }
	}
}
