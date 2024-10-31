using LazaProject.Core.DTO_S;
using LazaProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Application.IRepository
{
	public interface ICartRepository
	{
		Task<CartDTO> GetCartAsync (string UserId);
		Task AddToCartAsync(string  UserId, CartItemDTO cartItemDTO);
		//Task UpdateCartItemAsync(string  UserId, CartItemDTO cartItemDTO);
		Task RemoveFromCartAsync (string UserId,string ProductId);
		Task ClearCartAsync (string UserId);
	}
}
