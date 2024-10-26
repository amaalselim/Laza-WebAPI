using LazaProject.Application.IRepository;
using LazaProject.Core;
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
	public class UserRepository : IRepository<ApplicationUser>
	{
		private readonly ApplicationDbContext _context;

		public UserRepository(ApplicationDbContext context)
        {
			_context = context;
		}
        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
		{
			return await _context.applicationUsers.ToListAsync();
		}
		public async Task<ApplicationUser> GetByIdAsync(string id)
		{
			return await _context.applicationUsers.FindAsync(id);
		}
		public async Task AddAsync(ApplicationUser user)
		{
			await _context.applicationUsers.AddAsync(user);	
			await _context.SaveChangesAsync();	

		}
		public async Task UpdateAsync(ApplicationUser user)
		{
			_context.applicationUsers.Update(user);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(string id)
		{
			var user = await _context.applicationUsers.FindAsync(id);
			_context.applicationUsers.Remove(user);
			await _context.SaveChangesAsync();
		}


		
	}
}
