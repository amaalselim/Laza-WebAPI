using LazaProject.Application.IServices;
using LazaProject.Core.DTO_S;
using LazaProject.persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LazaAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderConfirmationmailController : ControllerBase
	{
		private readonly IEmailService _emailService;
		private readonly IOrderService _orderService;

		public OrderConfirmationmailController(IEmailService emailService,IOrderService orderService)
        {
			_emailService = emailService;
			_orderService = orderService;
		}
		[HttpPost("send-order-confirmation")]
		[Authorize]
		public async Task<IActionResult> SendOrderConfirmationEmail([FromBody] OrderConfirmationRequestDTO request)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
				var userName = User.FindFirstValue(ClaimTypes.Name); 
				var email = User.FindFirstValue(ClaimTypes.Email);

				var cart = await _orderService.GetCartByIdAsync(request.Cart.Id, userId);  
				var billingAddress = await _orderService.GetBillingAddressAsync(userId); 
				var paymentCard = await _orderService.GetPaymentCardAsync(userId); 

				await _emailService.SendOrderConfirmationEmailAsync(
					email,
					userName,
					cart,
					billingAddress,
					paymentCard
				);

				return Ok(new { message = "Order confirmation email sent successfully." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = $"Failed to send email: {ex.Message}" });
			}
		}
	}
}
