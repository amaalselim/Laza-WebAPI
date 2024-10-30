using LazaProject.Core.DTO_S;
using LazaProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Application.IRepository
{
    public interface IReviewRepository
    {
        Task AddReview(Reviews reviews);
		Task<IEnumerable<Reviews>> GetReviewsByProductIdAsync(string productId);
		Task<ReviewSummaryDTO> GetReviewSummaryByProductIdAsync(string productId);
	}
}
