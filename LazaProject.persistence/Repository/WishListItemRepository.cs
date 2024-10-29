using AutoMapper;
using LazaProject.Application.IRepository;
using LazaProject.Core.DTO_S;
using LazaProject.Core.Models;
using LazaProject.persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.persistence.Repository
{
	public class WishListItemRepository : IWishListItemRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

		public WishListItemRepository(ApplicationDbContext context,IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}
        public async Task<bool> AddToWishListAsync(string UserId, string ProductId)
		{

			var wishlistitem = new WishListItem
			{
				ProductId = ProductId,
				UserId = UserId
			};
			await _context.wishListItems.AddAsync(wishlistitem);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<IEnumerable<WishListItemDTO>> GetAllWishListAsync()
		{
			var wishlist= await _context.wishListItems.Select(w => new WishListItemDTO
			{
				UserId = w.UserId,
				ProductId = w.ProductId,
				ProductName = w.Product.Name,
				ProductPrice = w.Product.Price
			})
			.ToListAsync();
			return wishlist;
		}

		public async Task<IEnumerable<WishListItemDTO>> GetWishListItemByUserIdAsync(string UserId)
		{
			var wishlistItems = await _context.wishListItems
			.Where(w => w.UserId == UserId)
			.Select(w => new WishListItemDTO
			{
				UserId=w.UserId,
				ProductId= w.ProductId,
				ProductName=w.Product.Name,
				ProductPrice=w.Product.Price
			})
			.ToListAsync();
			return wishlistItems;
		}





		public async Task<bool> RemoveFromWishListAsync(string WishListItemId)
		{
			var item = await _context.wishListItems.FindAsync(WishListItemId);
			if (item == null)
			{
				return false;
			}
			 _context.wishListItems.Remove(item);
			return await _context.SaveChangesAsync() > 0;

		}
	}
}
