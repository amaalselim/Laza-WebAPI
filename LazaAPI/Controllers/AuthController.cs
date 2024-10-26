using LazaProject.Application.IRepository;
using LazaProject.Application.IUnitOfWork;
using LazaProject.Core.DTO_S;
using LazaProject.persistence.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LazaAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly AuthService _authService;

		public AuthController(IUnitOfWork unitOfWork,AuthService authService)
		{
			_unitOfWork = unitOfWork;
			_authService = authService;
		}

		[HttpPost("Register")]
		public async Task<IActionResult> Register(RegisterDTO registerDTO)
		{
			var result = await _unitOfWork.AuthRepo.RegisterAsync(registerDTO);
			if (result.Succeeded)
			{
				return Ok(new { Message = "User Registered Successfully." });
			}
			return BadRequest(result.Errors);
		}
		[HttpPost("Login")]

		public async Task<IActionResult> Login(LoginDTO loginDTO)
		{
			var user = await _unitOfWork.AuthRepo.LoginAsync(loginDTO);
			if (user != null)
			{
				return Ok(user);
			}
			return BadRequest("Invalid Login Attempt.");
		}

		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword(ForgetPasswordDTO forgetPasswordDTO)
		{
			var token=await _unitOfWork.AuthRepo.GeneratePasswordResetTokenAsync(forgetPasswordDTO);
			if (token==null)
			{
				return BadRequest("Failed to generate reset token.");
			}

			try
			{
				
				await _unitOfWork.AuthRepo.SendResetPasswordEmailAsync(forgetPasswordDTO.Email);
				return Ok(new { message = "Reset password email sent successfully." });
			}
			catch (Exception ex)
			{
		
				return StatusCode(500, $"Failed to send email: {ex.Message}");
			}

		}
		[HttpPost("update-password")]
		public async Task<IActionResult> UpdatePassword(UpdatePasswordDTO updatePasswordDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result=await _unitOfWork.AuthRepo.UpdatePasswordAsync(updatePasswordDTO);
			if (result.Succeeded)
			{
				return Ok(new { Message = "Password updated successfully." });
			}
			else
			{
				return BadRequest(result.Errors);
			}
			
		}

		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await _unitOfWork.AuthRepo.SignOutAsync();
			return Ok(new { Message = "User logged out successfully." });
		}

		[HttpPost("login-google")]
		public async Task<IActionResult> LoginWithGoogle([FromBody] GoogleLoginDTO googleLoginDTO)
		{
			var result = await _authService.LoginWithGoogleAsync(googleLoginDTO);
			if (result != null)
			{
				return Ok(result);
			}
			return BadRequest(new { message = "Invalid Google login." });
		}
		[HttpPost("Login-Facebook")]
		public async Task<IActionResult> LoginWithFacebook(FacebookLoginDTO facebookLoginDTO)
		{
			var result = await _authService.LoginWithFacebookAsync(facebookLoginDTO);

			if (result != null)
			{
				return Ok(result);
			}

			return BadRequest(new { message = "Invalid Facebook login." });
		}






	}

}
