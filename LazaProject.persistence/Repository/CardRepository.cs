using AutoMapper;
using LazaProject.Application.IRepository;
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
	public class CardRepository : ICardRepository
	{
		private readonly ApplicationDbContext _context;

		public CardRepository(ApplicationDbContext context)
        {
			_context = context;
		}
		public async Task<IEnumerable<Card>> GetAllCardsByUserIdAsync(string userId)
		{
			return await _context.cards.Where(c => c.UserId == userId).ToListAsync();
		}

		public async Task AddCardAsync(Card card)
		{
			await _context.cards.AddAsync(card);
			await _context.SaveChangesAsync();
		}
	}
}
