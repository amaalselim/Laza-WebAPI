using LazaProject.Application.IServices;
using LazaProject.Core.Models;
using LazaProject.persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.persistence.Services
{
	public class OrderService : IOrderService
	{
		private readonly ApplicationDbContext _context;

		public OrderService(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<AddressUser> GetBillingAddressAsync(string userId)
		{
			return await _context.address
								 .FirstOrDefaultAsync(a => a.UserId == userId);
		}

		public async Task<Cart> GetCartByIdAsync(string userId)
		{
			return await _context.carts
			.Include(c => c.Items.Where(i => i.IsActive))
			.ThenInclude(i => i.Product)
			.FirstOrDefaultAsync(c => c.UserId == userId);
		}

		public async Task<Card> GetPaymentCardAsync(string userId)
		{
			return await _context.cards
								 .FirstOrDefaultAsync(c => c.UserId == userId);
		}
	}
}
