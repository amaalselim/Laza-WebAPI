﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Core.DTO_S
{
	public class ProductDetailsDTO
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public List<ProductImgDTO> Images { get; set; }
		public List<ReviewDTO> Reviews { get; set; }
	}
}