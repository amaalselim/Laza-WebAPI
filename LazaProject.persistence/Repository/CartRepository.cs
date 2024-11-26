using AutoMapper;
using LazaProject.Application.IRepository;
using LazaProject.Core.DTO_S;
using LazaProject.Core.Models;
using LazaProject.persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LazaProject.persistence.Repository
{
	public class CartRepository : ICartRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

		public CartRepository(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task AddToCartAsync(string UserId, CartItemDTO cartItemDTO)
		{
			if (cartItemDTO.Quantity == 0)
			{
				cartItemDTO.Quantity = 1;
			}

			var cartDTo = await GetCartAsync(UserId);
			Cart cart;

			if (cartDTo.Items.Count==0)
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

				var product = await _context.products.FindAsync(cartItemDTO.ProductId);
				if (product != null)
				{
					cartItem.Product = product;
					cartItem.Price = product.Price;
				}

				cart.Items.Add(cartItem);
			}

			cart.TotalPrice = cart.Items.Sum(item => item.Quantity * item.Price);

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
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == UserId);

            if (cart == null)
            {
                
                return new CartDTO { Items = new List<CartItemDTO>() };
            }

            
            cart.Items = cart.Items.Where(i => i.IsActive).ToList();
            return _mapper.Map<CartDTO>(cart);
        }
        public async Task RemoveFromCartAsync(string UserId, string ProductId)
        {
            var cart = await _context.carts
                .FirstOrDefaultAsync(c => c.UserId == UserId);

            if (cart != null)
            {
                var cartItem = await _context.cartItems
                    .FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.ProductId == ProductId);

                if (cartItem != null)
                {
					_context.cartItems.Remove(cartItem);
                    await _context.SaveChangesAsync();
                }
            }
        }

    }

}
