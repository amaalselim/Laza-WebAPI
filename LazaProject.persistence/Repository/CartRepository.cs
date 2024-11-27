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

        public async Task AddToCartAsync(string userId, CartItemDTO cartItemDTO)
        {
            if (cartItemDTO.Quantity == 0)
            {
                cartItemDTO.Quantity = 1;
            }

            var cart = await _context.carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _context.carts.AddAsync(cart);
                await _context.SaveChangesAsync();
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == cartItemDTO.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += cartItemDTO.Quantity;
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
        public async Task<CartDTO> GetCartAsync(string userId)
        {
            var cart = await _context.carts
                .Include(c => c.Items.Where(i => i.IsActive))
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || cart.Items == null)
            {
                return new CartDTO { Items = new List<CartItemDTO>() };
            }

            return _mapper.Map<CartDTO>(cart);
        }



        public async Task RemoveFromCartAsync(string userId, string productId)
        {
           
            var cart = await _context.carts
                .Include(c => c.Items) 
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                throw new Exception("Cart not found for the specified user."); // خطأ إذا لم يتم العثور على العربة
            }
            var cartItem = cart.Items.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem != null)
            {
                _context.cartItems.Remove(cartItem);
                await _context.SaveChangesAsync(); 
            }
            else
            {
                throw new Exception("Item not found in the cart.");
            }
        }


    }

}
