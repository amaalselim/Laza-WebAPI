using AutoMapper;
using Facebook;
using Google.Apis.Auth;
using LazaProject.Application.IRepository;
using LazaProject.Application.IServices;
using LazaProject.Core.DTO_S;
using LazaProject.Core.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi;

namespace LazaProject.persistence.Repository
{
    public class AuthRepository : IAuthRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _manager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailservice;

        public AuthRepository(UserManager<ApplicationUser> user,
                              SignInManager<ApplicationUser> manager,
                              IMapper mapper,
                              IConfiguration config)
        {
            _userManager = user;
            _manager = manager;
            _mapper = mapper;
            _config = config;

            string smtpServer = _config["SMTP:Server"];
            int smtpPort = int.Parse(_config["SMTP:Port"]);
            string smtpUser = _config["SMTP:User"];
            string smtpPass = _config["SMTP:Password"];
            _emailservice = new EmailService(smtpServer, smtpPort, smtpUser, smtpPass);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string email, string token)
		{
			var user= await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				return IdentityResult.Failed(new IdentityError { Description = "User not found." });
			}
			var result= await _userManager.ConfirmEmailAsync(user, token);
			return result;
		}


		public async Task SendResetPasswordEmailAsync(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				return;
			}

			Random random = new Random();
			int verificationCode = random.Next(1000, 9999);

		
			user.VerificationCode = verificationCode;  
			await _userManager.UpdateAsync(user); 

			var resetLink = $"https://yourapp.com/reset-password?email={email}&code={verificationCode}";

			await _emailservice.SendEmailAsync(email,user.Name, "Reset Your Password",
			$"Your verification code is:<br/><code style='font-size: 18px; color: #3498db;'>{verificationCode}</code>");

		}




		public async Task<string> GeneratePasswordResetTokenAsync(ForgetPasswordDTO forgetPasswordDTO)
		{
			var user = await _userManager.FindByEmailAsync(forgetPasswordDTO.Email);
			if (user == null)
			{
				return null;
			}
			
			return await _userManager.GeneratePasswordResetTokenAsync(user);
		}



		public async Task<object> LoginAsync(LoginDTO loginDTO)
		{
			var user = await _userManager.FindByEmailAsync(loginDTO.Email);
			if (user != null)
			{
				
				var result = await _manager.PasswordSignInAsync(user.Email, loginDTO.Password, loginDTO.RememberMe, false);
				if (result.Succeeded)
				{
					List<Claim> claims = new List<Claim>
					{
						new Claim(ClaimTypes.Email, loginDTO.Email),
						new Claim(ClaimTypes.NameIdentifier, user.Id),
						new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
					};

					var roles = await _userManager.GetRolesAsync(user);
					foreach (var role in roles)
					{
						claims.Add(new Claim(ClaimTypes.Role, role));
					}

					SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
					SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

					var tokenExpiration = loginDTO.RememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddHours(20);

					JwtSecurityToken token = new JwtSecurityToken(
						issuer: _config["JWT:Issuer"],
						audience: _config["JWT:Audience"],
						expires: tokenExpiration,
						claims: claims,
						signingCredentials: signingCredentials
					);

					return new
					{
						Token = new JwtSecurityTokenHandler().WriteToken(token),
						Expire = tokenExpiration 
					};
				}
			}
			return new { Error = "Invalid email or password." };
		}


		public async Task<SignInResult> PasswordSignInAsync(LoginDTO loginDto)
		{
			return await _manager.PasswordSignInAsync(loginDto.Email,loginDto.Password,loginDto.RememberMe,false);
		}

		public async Task<IdentityResult> RegisterAsync(RegisterDTO registerDTO)
		{
			if(!await IsEmailValid(registerDTO.Email))
			{
				return IdentityResult.Failed(new IdentityError { Description = "Email Is Not Valid , Please Enter a Valid Email." });
			}
			var user= _mapper.Map<ApplicationUser>(registerDTO);
			return await _userManager.CreateAsync(user, registerDTO.Password);

		}



		private async Task<bool> IsEmailValid(string email)
		{
			string apiKey = "6ad555fea7f14c12b583131f2e44b27b"; 
			string apiUrl = $"https://api.zerobounce.net/v2/validate?api_key={apiKey}&email={email}";

			using (var client = new HttpClient())
			{
				var response = await client.GetStringAsync(apiUrl);
				var result = JObject.Parse(response);

				return result["status"]?.ToString() == "valid";
			}
		}


		public async Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword)
		{
			var user= await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				return IdentityResult.Failed(new IdentityError { Description = "User not found." });
			}
			var result= await _userManager.ResetPasswordAsync(user, token, newPassword);
			return result;
		}

		public async Task SignOutAsync()
		{
			await _manager.SignOutAsync();
		}

		
		public async Task<IdentityResult> UpdatePasswordAsync(UpdatePasswordDTO updatePasswordDTO)
		{
			var user= await _userManager.FindByEmailAsync(updatePasswordDTO.Email);
			if (user == null)
			{
				return IdentityResult.Failed(new IdentityError { Description = "User not found." });
			}
			if (user.VerificationCode.ToString() != updatePasswordDTO.VerificationCode)
			{
				return IdentityResult.Failed(new IdentityError { Description = "User not found." });
			}
			var token=await _userManager.GeneratePasswordResetTokenAsync(user);
			var result=await _userManager.ResetPasswordAsync(user,token,updatePasswordDTO.NewPassword);

			if (result.Succeeded)
			{
				user.VerificationCode = null;
				await _userManager.UpdateAsync(user);
			}
			return result;
		}

		public async Task<object> LoginWithGoogleAsync(GoogleLoginDTO googleLoginDTO)
		{
			var payload=await GoogleJsonWebSignature.ValidateAsync(googleLoginDTO.IdToken);
			if(payload == null)
			{
				throw new Exception("Invalid Google ID Token");
			}
			var user = await _userManager.FindByEmailAsync(payload.Email);
			if (user == null)
			{
				user = new ApplicationUser
				{
					Name=payload.Name,
					UserName = payload.Email,
					Email = payload.Email
	
				};
				await _userManager.CreateAsync(user);	
			}
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expiration = DateTime.Now.AddDays(30);

			var token = new JwtSecurityToken(
				issuer: _config["JWT:Issuer"],
				audience: _config["JWT:Audience"],
				claims: claims,
				expires: expiration,
				signingCredentials: creds);

			return new
			{
				Token = new JwtSecurityTokenHandler().WriteToken(token),
				Expiration = expiration
			};
		}

		public async Task<object> LoginWithFacebookAsync(FacebookLoginDTO facebookLoginDTO)
		{
			var fbClient = new FacebookClient(facebookLoginDTO.AccessToken);
			dynamic userInfo = await fbClient.GetTaskAsync("me?fields=id,name,email");

			var email = userInfo.email;

			if (string.IsNullOrEmpty(email))
			{
				throw new ArgumentNullException(nameof(email), "Email cannot be null or empty.");
			}
			var user=await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				user = new ApplicationUser
				{
					Name = userInfo.Name,
					UserName = email,
					Email = email
				};
				await _userManager.CreateAsync(user);
			}
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expiration = DateTime.Now.AddDays(30);

			var token = new JwtSecurityToken(
				issuer: _config["JWT:Issuer"],
				audience: _config["JWT:Audience"],
				claims: claims,
				expires: expiration,
				signingCredentials: creds);

			return new
			{
				Token = new JwtSecurityTokenHandler().WriteToken(token),
				Expiration = expiration
			};
		}
		public async Task<object> LoginWithTwitterAsync(TwitterLoginDTO twitterLoginDTO)
		{
			var consumerKey = _config["Twitter:ConsumerKey"];
			var consumerSecret = _config["Twitter:ConsumerSecret"];


			var appCredentials = new TwitterCredentials(consumerKey, consumerSecret, twitterLoginDTO.AccessToken, twitterLoginDTO.AccessTokenSecret);
			var userClient = new TwitterClient(appCredentials);


			var twitterUser = await userClient.Users.GetAuthenticatedUserAsync();

			if (twitterUser == null)
			{
				throw new Exception("Invalid Twitter Access Token");
			}

			var email = twitterUser.Email; 
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				user = new ApplicationUser
				{
					Name = twitterUser.Name,
					UserName = email,
					Email = email
				};
				await _userManager.CreateAsync(user);
			}

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expiration = DateTime.Now.AddDays(30);

			var token = new JwtSecurityToken(
				issuer: _config["JWT:Issuer"],
				audience: _config["JWT:Audience"],
				claims: claims,
				expires: expiration,
				signingCredentials: creds);

			return new
			{
				Token = new JwtSecurityTokenHandler().WriteToken(token),
				Expiration = expiration
			};
		}

		
	}

}
