using LazaProject.Core.DTO_S;
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
		Task<CartDTO> GetCartByIdAsync(string userId);
		Task<List<CartDTO>> GetCartsByUserIdAsync(string userId);

        Task<AddressUser> GetBillingAddressAsync(string userId);
		Task<Card> GetPaymentCardAsync(string userId);
	}

}
