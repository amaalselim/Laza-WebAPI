using Google.Apis.Gmail.v1.Data;
using LazaProject.Application.IUnitOfWork;
using LazaProject.Core.DTO_S;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LazaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
		private readonly IUnitOfWork _unitOfWork;

		public CartController(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
		}
        [HttpGet("GetCart")]
        public async Task<ActionResult<CartDTO>> GetCart()
        {
            var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart=await _unitOfWork.cartRepository.GetCartAsync(userId);
            if (cart == null)
            {
				return Ok(new { Message = "No items available in the cart." });
			}
            return Ok(cart);
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDTO cartItemDTO)
        {
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _unitOfWork.cartRepository.AddToCartAsync(userId,cartItemDTO);
			return CreatedAtAction(nameof(GetCart), new { userId = userId }, cartItemDTO);
		}
		[HttpDelete("Remove-items/{productId}")]
		public async Task<IActionResult> RemoveFromCart(string productId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			await _unitOfWork.cartRepository.RemoveFromCartAsync(userId, productId);
			return Ok("Item Removed From Cart Successfully");
		}
		[HttpDelete("Clear-Cart")]
		public async Task<IActionResult> ClearCart()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			await _unitOfWork.cartRepository.ClearCartAsync(userId);
			return Ok("Cart Cleared Successfully");
		}

	}
}
