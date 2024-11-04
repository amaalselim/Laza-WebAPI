using AutoMapper;
using LazaProject.Application.IUnitOfWork;
using LazaProject.Core.DTO_S;
using LazaProject.Core.Models;
using LazaProject.persistence.Migrations;
using LazaProject.persistence.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LazaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	[Authorize]
    public class ReviewController : ControllerBase
    {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ReviewController(IUnitOfWork unitOfWork,IMapper mapper)
        {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		[HttpPost("AddReview/{productId}")]
		public async Task<IActionResult> AddReview(string productId, [FromBody] ReviewDTO reviewDTO)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var review = new Reviews
			{
				productId= productId,
				UserId=userId,
				UserName=reviewDTO.Username,
				Feedback=reviewDTO.Feedback,
				Rating=reviewDTO.Rating
			};
			await _unitOfWork.ReviewRepository.AddReview(review);
			return CreatedAtAction(nameof(GetProductReviews), new { productId = productId }, review);


		}
		[HttpGet("All-reviews/{productId}")]
		public async Task<IActionResult> GetProductReviews(string productId)
		{
			var reviews= await _unitOfWork.ReviewRepository.GetReviewsByProductIdAsync(productId);
			if(reviews==null || !reviews.Any())
			{
				return NotFound("No reviews found for this product.");
			}
			var reviewsDTO=_mapper.Map<IEnumerable<ReviewDTO>>(reviews);	
			return Ok(reviewsDTO);
		}

		[HttpGet("summary/{productId}")]
		public async Task<IActionResult> GetReviewSummary (string productId)
		{
			var reviewSummary = await _unitOfWork.ReviewRepository.GetReviewSummaryByProductIdAsync(productId);
			if (reviewSummary == null)
			{
				return NotFound("No reviews found for this product.");
			}
			return Ok(reviewSummary);
		}



	}
}
