using LazaProject.Core.DTO_S;
using LazaProject.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Application.IRepository
{
    public interface IAuthRepo
	{
		
		Task<IdentityResult> RegisterAsync (RegisterDTO registerDTO);
		Task<object> LoginAsync (LoginDTO loginDTO);
		Task<string> GeneratePasswordResetTokenAsync(ForgetPasswordDTO forgetPasswordDTO);
		Task<IdentityResult> UpdatePasswordAsync(UpdatePasswordDTO updatePasswordDTO);
		Task SendResetPasswordEmailAsync (string email);	
		Task SignOutAsync();


		Task<object> LoginWithGoogleAsync (GoogleLoginDTO googleLoginDTO);
		Task<object> LoginWithFacebookAsync (FacebookLoginDTO facebookLoginDTO);
		Task<object> LoginWithTwitterAsync(TwitterLoginDTO twitterLoginDTO);

		Task<bool> AssignRoleAsync(AssignRoleDto assignRoleDto);



	}
}
