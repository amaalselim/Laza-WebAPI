using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Core.Models
{
	public class Cart
	{
		public string Id { get; set; }= Guid.NewGuid().ToString();
		public string UserId { get;set; }
		public List<CartItem> Items { get; set; }= new List<CartItem>();
		public decimal TotalPrice { get; set; }
		 
	}
}
