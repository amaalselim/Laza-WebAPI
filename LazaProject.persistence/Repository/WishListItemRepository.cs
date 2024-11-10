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
				UserId = UserId,
			};
			await _context.wishListItems.AddAsync(wishlistitem);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<IEnumerable<WishListItemDTO>> GetWishListItemByUserIdAsync(string UserId)
		{
			var wishlistItems = await _context.wishListItems
			.Where(w => w.UserId == UserId)
			.Select(w => new WishListItemDTO
			{
				UserId=w.UserId,
				Id= w.ProductId,
				Name=w.Product.Name,
				Price=w.Product.Price,
				Img=w.Product.Img
			})
			.ToListAsync();
			return wishlistItems;
		}





		public async Task<bool> RemoveFromWishListAsync(string ProductId)
		{
			var item = await _context.wishListItems.Where(w=>w.ProductId == ProductId).FirstOrDefaultAsync();	
			if (item == null)
			{
				return false;
			}
			 _context.wishListItems.Remove(item);
			return await _context.SaveChangesAsync() > 0;

		}
	}
}
