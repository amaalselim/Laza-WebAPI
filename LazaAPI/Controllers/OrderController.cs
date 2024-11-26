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
using LazaProject.Application.IUnitOfWork;
using AutoMapper;

namespace LazaAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [Authorize]
    public class OrderController : ControllerBase
	{
		private readonly IEmailService _emailService;
		private readonly IOrderService _orderService;
		private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public OrderController(
			IEmailService emailService,
			IOrderService orderService,
			Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
			IMapper mapper
			)
		{
			_emailService = emailService;
			_orderService = orderService;
			_userManager = userManager;
            _mapper = mapper;
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
				var Cartdto=_mapper.Map<CartDTO>(cart);
				var billingAddress = await _orderService.GetBillingAddressAsync(userId);
				var paymentCard = await _orderService.GetPaymentCardAsync(userId);

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
					return StatusCode(500, new { message = "Failed to send email." });
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = $"Failed to send email: {ex.Message}" });
			}
		}
		[HttpGet("GetOrder")]
		public async Task<IActionResult> GetOrder()
		{
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var order = await _orderService.GetCartsByUserIdAsync(userId);
            if (order == null)
            {
                return Ok(new { Message = "No Orders Available" });
            }
            return Ok(order);
        }


	}
}
