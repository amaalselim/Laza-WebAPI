using LazaProject.Application.IServices;

using Microsoft.AspNetCore.Identity;
using LazaProject.Core.DTO_S;
using LazaProject.Core.Models;
using LazaProject.persistence.Services;
using Microsoft.AspNet.Identity;
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
		private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

		public OrderConfirmationmailController(IEmailService emailService, IOrderService orderService, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
		{
			_emailService = emailService;
			_orderService = orderService;
			_userManager = userManager;
			_userManager = userManager;
		}
		[HttpPost("send-order-confirmation")]
		[Authorize]
		public async Task<IActionResult> SendOrderConfirmationEmail()
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				var user = await _userManager.FindByIdAsync(userId);
				var email = user.Email;
				var username = user.Name;

				var cart = await _orderService.GetCartByIdAsync(userId);
				var billingAddress = await _orderService.GetBillingAddressAsync(userId);
				var paymentCard = await _orderService.GetPaymentCardAsync(userId);

				// Send the email and check if it was successful
				bool emailSent = await _emailService.SendOrderConfirmationEmailAsync(
					email,
					username,
					cart,
					billingAddress,
					paymentCard
				);

				if (emailSent)
				{
					return Ok(new { message = "Order confirmation email sent successfully." });
				}
				else
				{
					// If email sending failed
					return StatusCode(500, new { message = "Failed to send email." });
				}
			}
			catch (Exception ex)
			{
				// Handle unexpected exceptions
				return StatusCode(500, new { message = $"Failed to send email: {ex.Message}" });
			}
		}

	}
}
