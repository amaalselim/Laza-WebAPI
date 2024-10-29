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
	public class CategoryRepository : IRepository<Category>
	{
		private readonly ApplicationDbContext _context;

		public CategoryRepository(ApplicationDbContext Context)
        {
			_context = Context;
		}
        public async Task AddAsync(Category category)
		{
			await _context.categories.AddAsync(category);
			await _context.SaveChangesAsync();	
		}

		public async Task DeleteAsync(string id)
		{
			var cat=await _context.categories.FindAsync(id);
			_context.categories.Remove(cat);
			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Category>> GetAllAsync()
		{
			List<Category> cat = await _context.categories.ToListAsync();
			return cat;
		}

		public async Task<Category> GetByIdAsync(string id)
		{
			var cat = await _context.categories.FindAsync(id);
			return cat;
		}

		public async Task UpdateAsync(Category category)
		{
			_context.Update(category);	
			await _context.SaveChangesAsync();
		}
	}
}
