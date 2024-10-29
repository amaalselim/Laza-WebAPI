using LazaProject.Core.DTO_S;
using LazaProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Application.IRepository
{
	public interface IWishListItemRepository
	{
		Task<bool> AddToWishListAsync(string UserId, string ProductId);
		Task<bool> RemoveFromWishListAsync(string WishListItemId);
		Task<IEnumerable<WishListItemDTO>> GetWishListItemByUserIdAsync(string UserId);	
		Task<IEnumerable<WishListItemDTO>> GetAllWishListAsync();

	}
}
