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
	public class productImageRepository: IProductImageRepository
	{
		private readonly ApplicationDbContext _context;

		public productImageRepository(ApplicationDbContext Context)
        {
			_context = Context;
		}

		public async Task AddAsync(productImage entity)
		{
			await _context.productImages.AddAsync(entity);
			await _context.SaveChangesAsync();
		}

		public async Task AddRangeAsync(IEnumerable<productImage> productImages)
		{
			await _context.productImages.AddRangeAsync(productImages);
		}

		public Task DeleteAsync(string id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<productImage>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public async Task<productImage> GetByIdAsync(string id)
		{
			var proimg=await _context.productImages.FindAsync(id);
			return proimg;
		}
		


		public Task UpdateAsync(productImage entity)
		{
			throw new NotImplementedException();
		}

		
	}
}
