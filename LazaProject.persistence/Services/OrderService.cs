using AutoMapper;
using LazaProject.Application.IServices;
using LazaProject.Core.DTO_S;
using LazaProject.Core.Models;
using LazaProject.persistence.Data;
using LazaProject.persistence.Migrations;
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
        private readonly IMapper _mapper;

        public OrderService(ApplicationDbContext context,IMapper mapper)
		{
			_context = context;
            _mapper = mapper;
        }
		public async Task<AddressUser> GetBillingAddressAsync(string userId)
		{
			return await _context.address
								 .FirstOrDefaultAsync(a => a.UserId == userId);
		}

		public async Task<CartDTO> GetCartByIdAsync(string userId)
		{
			var cart= await _context.carts
			.Include(c => c.Items.Where(i => !i.IsActive))
			.ThenInclude(i => i.Product)
			.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null || cart.Items.Any(i => i.IsActive))
            {
                return null;
            }
            return _mapper.Map<CartDTO>(cart);
        }

		public async Task<Card> GetPaymentCardAsync(string userId)
		{
			return await _context.cards
								 .FirstOrDefaultAsync(c => c.UserId == userId);
		}
	}
}
