using LazaProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Application.IServices
{
	public interface IOrderService
	{
		Task<Cart> GetCartByIdAsync(string cartId, string userId);
		Task<AddressUser> GetBillingAddressAsync(string userId);
		Task<Card> GetPaymentCardAsync(string userId);
	}

}
