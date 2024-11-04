using AutoMapper;
using LazaProject.Application.IRepository;
using LazaProject.Core.DTO_S;
using LazaProject.Core.Enums;
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
			return await _context.products.ToListAsync();

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

		public async Task<IEnumerable<Product>> GetAllProAsync(Gender? userGender)
		{
			if (userGender == Gender.Men)
			{
				return await _context.products.Where(p => p.type == ProductType.Men).ToListAsync();
			}
			else if (userGender == Gender.Women)
			{
				return await _context.products.Where(p => p.type == ProductType.Women).ToListAsync();
			}
			else
			{
				return await _context.products.ToListAsync();
			}
		}

		public async Task<IEnumerable<Product>> GetAllProductByCategoryIdAsync(string categoryid,Gender? gender)
		{
			if (gender == Gender.Men)
			{
				return await _context.products.Include(p => p.Category)
				.Where(p => p.CategoryId == categoryid)
				.Where(p=>p.type==ProductType.Men)
				.ToListAsync();
			}
			else if (gender== Gender.Women)
			{
				return await _context.products.Include(p => p.Category)
				.Where(p => p.CategoryId == categoryid)
				.Where(p => p.type == ProductType.Women)
				.ToListAsync();
			}
			else
			{
				return await _context.products.Include(p => p.Category)
				.Where(p => p.CategoryId == categoryid)
				.ToListAsync();
			}
		}

		public async Task<Product> GetByIdAsync(string id)
		{
			var product = await _context.products.FindAsync(id);

			return product;
		}

		public async Task<ProductDetailsDTO> GetImagesByProductIdAsync(string id)
		{
			var product = await _context.products
				.AsNoTracking()
				.Include(p => p.Images)
				.Include(p => p.Category)
				.Include(p => p.Reviews) 
				.FirstOrDefaultAsync(p => p.Id == id);

			if (product == null)
			{
				return null;
			}
			var productdetails = new ProductDetailsDTO
			{
				Id = product.Id,
				Name = product.Name,
				Images = product.Images.Where(img => !img.Image.StartsWith("data:image"))
				.Select(img => new ProductImgDTO
				{
					Image = img.Image,
					ProductId = product.Id,
					ProductName = product.Name
				}).ToList(),
				Reviews = product.Reviews.Select(r => new ReviewDTO
				{
					UserId = r.UserId,
					Feedback = r.Feedback,
					Rating = r.Rating
				}).ToList()
			};
			return productdetails;
		}


		public async Task<IEnumerable<Product>> GetProductsSortedByPrice()
		{
			return await _context.products.OrderBy(p => p.Price).ToListAsync();
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
