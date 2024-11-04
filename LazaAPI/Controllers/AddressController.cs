using AutoMapper;
using Google.Apis.Gmail.v1.Data;
using LazaProject.Application.IUnitOfWork;
using LazaProject.Core.DTO_S;
using LazaProject.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LazaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public AddressController(IUnitOfWork unitOfWork,IMapper mapper)
        {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		[HttpPost("Add-Address")]
		public async Task<IActionResult> AddAddress([FromBody]AddressDTO addressDTO)
		{
			if (!User.Identity.IsAuthenticated)
			{
				return Unauthorized(new { message = "Please log in to continue." });
			}
			var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user =await _unitOfWork.Users.GetByIdAsync(userId);

			var add = new AddressUser
			{
				UserId = userId,
				UserName = user.UserName,
				Address = addressDTO.Address,
				City = addressDTO.City,
				Country = addressDTO.Country,
				PhoneNumber = addressDTO.Phone
			};
			_unitOfWork.AddressRepository.AddAsync(add);
			return Ok(new { Message = "Address Added To User successfully!" });
		}
    }
}
