using AutoMapper;
using LazaProject.Application.IRepository;
using LazaProject.Core.DTO_S;
using LazaProject.Core.Enums;
using LazaProject.Core.Models;
using LazaProject.persistence.Data;
using LazaProject.persistence.Migrations;
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
				product.Images = product.Images.Select(img => new productImage
				{
					Image = img.Image,
					ProductId = product.Id,
					ProductName = product.Name
				}).ToList();
			}

			return pro;
		}

		public async Task<IEnumerable<ProductDetailsDTO>> GetAllProAsync()
		{
			var query = _context.products.AsNoTracking()
				.Include(p => p.Images)
				.Include(p => p.Category)
				.Include(p => p.Reviews)
				.AsQueryable();

			var productsByCategory = await query
				.GroupBy(p => p.CategoryId)
				.ToListAsync();

			var productDetailsList = new List<ProductDetailsDTO>();

			foreach (var categoryGroup in productsByCategory)
			{
				var products = categoryGroup.Skip(2).Take(4).ToList();

				productDetailsList.AddRange(products.Select(product => new ProductDetailsDTO
				{
					Id = product.Id,
					Name = product.Name,
					Description = product.Description,
					Price = product.Price,
					Img = product.Img,
					CategoryId = product.CategoryId,
					Images = product.Images.Select(img => new ProductImgDTO
					{
						Image = img.Image
					}).ToList(),
					Reviews = product.Reviews.Select(r => new ReviewDTO
					{
						UserId = r.UserId,
						Username = r.UserName,
						Feedback = r.Feedback,
						Rating = r.Rating
					}).ToList()
				}));
			}

			return productDetailsList;
		}

		public async Task<IEnumerable<ProductDetailsDTO>> GetAllProductByCategoryIdAsync(string categoryid)
			{
			var query = _context.products.AsNoTracking()
				.Include(p => p.Images)
				.Include(p => p.Category)
				.Include(p => p.Reviews)
				.Where(p => p.CategoryId == categoryid);

			var products = await query.ToListAsync();

			var productDetailsList = products.Select(product => new ProductDetailsDTO
			{
				Id = product.Id,
				Name = product.Name,
				Description = product.Description,
				Price = product.Price,
				Img = product.Img,
				CategoryId = product.CategoryId,
				Images = product.Images.Select(img => new ProductImgDTO
				{
					Image = img.Image
				}).ToList(),
				Reviews = product.Reviews.Select(r => new ReviewDTO
				{
					UserId = r.UserId,
					Username = r.UserName,
					Feedback = r.Feedback,
					Rating = r.Rating
				}).ToList()
			}).ToList();

			return productDetailsList;
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
				Description = product.Description,
				Price = product.Price,
				CategoryId = product.CategoryId,
				Img=product.Img,
				Images = product.Images
				.Select(img => new ProductImgDTO
				{
					Image = img.Image
				}).ToList(),
				Reviews = product.Reviews.Select(r => new ReviewDTO
				{
					UserId = r.UserId,
					Username = r.UserName,
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
			return await _context.products
				.Where(p => p.Name.ToLower().StartsWith(lowersearchterm)).ToListAsync();
		}
		public async Task UpdateAsync(Product product)
		{
			_context.products.Update(product);
			await _context.SaveChangesAsync();
		}
	}
}
