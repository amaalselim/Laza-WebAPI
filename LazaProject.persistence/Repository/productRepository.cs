using AutoMapper;
using LazaProject.Application.IRepository;
using LazaProject.Core.Models;
using LazaProject.persistence.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.persistence.Repository
{
	public class productRepository : IProductRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

		public productRepository(ApplicationDbContext Context,IMapper mapper)
        {
			_context = Context;
			_mapper = mapper;
		}
        public async Task AddAsync(Product product)
		{

			await _context.products.AddAsync(product);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(string id)
		{
			var prod = await _context.products.FindAsync(id);
			_context.products.Remove(prod);
			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Product>> GetAllAsync()
		{
			List<Product> pro = await _context.products.ToListAsync();
			return pro;

		}

		public async Task<IEnumerable<Product>> GetAllImagesByProducts()
		{
			List<Product> pro = await _context.products.Include(p => p.Images).Include(p => p.Category).ToListAsync();
			foreach (var product in pro)
			{
				product.Images = product.Images.Where(img => !img.Image
				.StartsWith("data:image")).Select(img => new productImage
				{
					Image = img.Image,
					ProductId = product.Id,
					ProductName = product.Name
				}).ToList();
			}

			return pro;
		}

		public async Task<IEnumerable<Product>> GetAllProductByCategoryIdAsync(string categoryid)
		{
			return await _context.products.Include(p=>p.Category)
				.Where(p=>p.CategoryId==categoryid)
				.ToListAsync();

		}

		public async Task<Product> GetByIdAsync(string id)
		{
			var product = await _context.products.FindAsync(id);

			return product;
		}

		public async Task<Product> GetImagesByProductIdAsync(string id)
		{
			var product = await _context.products
				.AsNoTracking()
				.Include(p => p.Images).Include(p => p.Category)
				.FirstOrDefaultAsync(p => p.Id == id);

			if (product == null)
			{
				return null;
			}
			product.Images = product.Images.Where(img => !img.Image
				.StartsWith("data:image")).Select(img => new productImage
				{
					Image = img.Image,
					ProductId = product.Id,
					ProductName = product.Name
				}).ToList();

			return product;
		}

		public async Task<IEnumerable<Product>> SearchProductAsync(string searchTerm)
		{
			var lowersearchterm=searchTerm.ToLower();
			return await _context.products.Include(p => p.Category)
				.Where(p => p.Name.ToLower().StartsWith(lowersearchterm) || 
				p.Category.Name.ToLower().StartsWith(lowersearchterm)).ToListAsync();
		}
		public async Task UpdateAsync(Product product)
		{
			_context.products.Update(product);
			await _context.SaveChangesAsync();
		}
	}
}
