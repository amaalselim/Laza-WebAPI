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
	public class CartRepository : ICartRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

		public CartRepository(ApplicationDbContext context,IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}
		public async Task AddToCartAsync(string UserId, CartItemDTO cartItemDTO)
		{
			var cartDTo = await GetCartAsync(UserId);
			Cart cart;

			if (cartDTo == null)
			{
				cart = new Cart { UserId = UserId };
				await _context.carts.AddAsync(cart);
				await _context.SaveChangesAsync(); 
			}
			else
			{
				cart = await _context.carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == UserId);
			}

			var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == cartItemDTO.ProductId);
			if (existingItem != null)
			{
				existingItem.Quantity = cartItemDTO.Quantity;
				existingItem.Price = existingItem.Product.Price; 
			}
			else
			{
				var cartItem = _mapper.Map<CartItem>(cartItemDTO);
				cartItem.CartId = cart.Id; 
				cart.Items.Add(cartItem); 
			}
			cart.TotalPrice = cart.Items.Sum(item => item.Quantity * item.Product.Price);

			_context.carts.Update(cart);
			await _context.SaveChangesAsync();
		}


		public async Task ClearCartAsync(string UserId)
		{
			var cart = await _context.carts
				.Include(c => c.Items)
				.FirstOrDefaultAsync(c => c.UserId == UserId);

			if (cart != null)
			{
				foreach (var item in cart.Items)
				{
					item.IsActive = false;
				}
				await _context.SaveChangesAsync();
			}
		}


		public async Task<CartDTO> GetCartAsync(string UserId)
		{
			var cart = await _context.carts
				.Include(c => c.Items.Where(i => i.IsActive)) 
				.ThenInclude(p => p.Product)
				.FirstOrDefaultAsync(c => c.UserId == UserId);

			if (cart == null || !cart.Items.Any(i => i.IsActive))
			{
				return null; 
			}

			return _mapper.Map<CartDTO>(cart);
		}



		public async Task RemoveFromCartAsync(string UserId, string ProductId)
		{
			var cart = await _context.carts
				.Include(c => c.Items)
				.FirstOrDefaultAsync(c => c.UserId == UserId);

			if (cart == null) return;

			var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == ProductId && i.IsActive);
			if (cartItem != null)
			{
				cartItem.IsActive = false; 
				await _context.SaveChangesAsync();
			}
		}


		//public async Task UpdateCartItemAsync(string UserId, CartItemDTO cartItemDTO)
		//{
		//	var cart = await GetCartAsync(UserId);
		//	if (cart == null) return;

		//	var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == cartItemDTO.ProductId);
		//	if (cartItem != null)
		//	{
		//		cartItem.Quantity = cartItemDTO.Quantity;
		//		await _context.SaveChangesAsync();
		//	}
		//}
	}
}
