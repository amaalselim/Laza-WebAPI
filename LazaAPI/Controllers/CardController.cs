using AutoMapper;
using LazaProject.Application.IUnitOfWork;
using LazaProject.Core.DTO_S;
using LazaProject.Core.Models;
using LazaProject.persistence.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tweetinvi.Core.Models;

namespace LazaAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class CardController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CardController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		[HttpGet("GetCard")]
		public async Task<IActionResult> GetAllCardsByUserId()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrWhiteSpace(userId))
			{
				return Unauthorized(new { message = "Please log in to continue." });
			}
			var cards = await _unitOfWork.CardRepository.GetAllCardsByUserIdAsync(userId);
			var cardDTOs = _mapper.Map<IEnumerable<CardDTO>>(cards);
			return Ok(cardDTOs);
		}

		[HttpPost("AddNewCard")]
		public async Task<IActionResult> AddCard([FromBody] CardDTO cardDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrWhiteSpace(userId))
			{
				return Unauthorized(new { message = "Please log in to continue." });
			}
			cardDTO.UserId = userId;

			var card = _mapper.Map<Card>(cardDTO);
			await _unitOfWork.CardRepository.AddCardAsync(card);

			return CreatedAtAction(nameof(GetAllCardsByUserId), new { userId = card.UserId }, cardDTO);
		}
	}
}
