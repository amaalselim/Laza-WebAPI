using LazaProject.Application.IRepository;
using LazaProject.Core.DTO_S;
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
	public class ReviewRepository : IReviewRepository
	{
		private readonly ApplicationDbContext _context;

		public ReviewRepository(ApplicationDbContext context)
        {
			_context = context;
		}
        public async Task AddReview(Reviews reviews)
		{
			await _context.reviews.AddAsync(reviews);
			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Reviews>> GetReviewsByProductIdAsync(string productId)
		{
			return await _context.reviews.Where(p=>p.productId==productId)
				.ToListAsync();
		}

		public async Task<ReviewSummaryDTO> GetReviewSummaryByProductIdAsync(string productId)
		{
			var reviews = await _context.reviews.Where(r => r.productId == productId).ToListAsync();
			if (reviews == null || !reviews.Any()) return null; 

			var AverageRating = reviews.Average(p => p.Rating);
			var reviewcount = reviews.Count;

			var reviewsummary = new ReviewSummaryDTO
			{
				AverageRating= AverageRating,
				ReviewCount= reviewcount
			};
			return reviewsummary;



		}
	}
}
