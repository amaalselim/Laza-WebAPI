﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LazaProject.Core.Models
{
	public class Category
	{
		public string? Id { get; set; }= Guid.NewGuid().ToString();
		[JsonIgnore]
		public virtual ICollection<Product>? Products { get; set; }
	}
}
