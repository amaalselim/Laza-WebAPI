using LazaProject.Application.IUnitOfWork;
using LazaProject.Core.DTO_S;
using LazaProject.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LazaAPI.Controllers

{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class WishListController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;

		public WishListController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		[HttpPost("Add-WishList")]
		public async Task<IActionResult> AddToWishList([FromBody]WishListItemDTO wishListItemDTO)
		{
			if (wishListItemDTO == null)
			{
				return BadRequest("Wishlist item cannot be null.");
			}

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrWhiteSpace(userId))
			{
				return Unauthorized("User not authenticated.");
			}
			wishListItemDTO.UserId = userId;
			var result = await _unitOfWork.WishListItemRepository.AddToWishListAsync(wishListItemDTO.UserId, wishListItemDTO.ProductId);

			if (result)
			{
				return Ok("Product added to wishlist successfully.");
			}
			else
			{
				return BadRequest("Could not add to wishlist.");
			}
		}

		[HttpGet("user/{userId}")]
		public async Task<IActionResult> GetUserWishList(string userId)
		{
			if (string.IsNullOrWhiteSpace(userId))
			{
				return BadRequest("UserId cannot be empty.");
			}

			var wishlistItems = await _unitOfWork.WishListItemRepository.GetWishListItemByUserIdAsync(userId);

			if (wishlistItems == null || !wishlistItems.Any())
			{
				return NotFound("No wishlist items found for this user.");
			}

			return Ok(wishlistItems);
		}

		[HttpGet("users")]
		public async Task<IActionResult> GetAllWishListItems()
		{
			var wishlistItems = await _unitOfWork.WishListItemRepository.GetAllWishListAsync();

			if (wishlistItems == null)
			{
				return NotFound("No wishlist items found");
			}

			return Ok(wishlistItems);
		}


		[HttpDelete("Remove/{id}")]
		public async Task<IActionResult> RemoveFromWishList(string id)
		{
			var result = await _unitOfWork.WishListItemRepository.RemoveFromWishListAsync(id);
			if (!result)
			{
				return NotFound("Wishlist item not found.");
			}
			return Ok("Wishlist item removed successfully.");
		}
	}
}
