using LazaProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Core.DTO_S
{
	public class OrderConfirmationRequestDTO
	{
		public string Email { get; set; }
		public string UserName { get; set; }
		public Cart Cart { get; set; }
		public AddressUser BillingAddress { get; set; }
		public Card PaymentCard { get; set; }
	}
}
