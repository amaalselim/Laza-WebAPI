using LazaProject.Application.IRepository;
using LazaProject.Core.DTO_S;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.persistence.Services
{
	public class AuthService
	{
		private readonly IAuthRepo _authRepo;

		public AuthService(IAuthRepo authRepo)
        {
			_authRepo = authRepo;
		}
		public async Task<object> LoginWithGoogleAsync(GoogleLoginDTO googleLoginDTO)
		{
			return await _authRepo.LoginWithGoogleAsync(googleLoginDTO);
		}
		public async Task<object> LoginWithFacebookAsync(FacebookLoginDTO facebookLoginDTO)
		{
			return await _authRepo.LoginWithFacebookAsync(facebookLoginDTO);
		}

	}
}
