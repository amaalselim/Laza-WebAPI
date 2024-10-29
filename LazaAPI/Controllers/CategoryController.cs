using LazaProject.Application.IServices;
using LazaProject.Application.IUnitOfWork;
using LazaProject.Core.DTO_S;
using LazaProject.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LazaAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IImageService _imageService;

		public CategoryController(IUnitOfWork unitOfWork, IImageService imageService)
		{
			_unitOfWork = unitOfWork;
			_imageService = imageService;
		}
		[HttpGet]
		public async Task<IActionResult> ViewAllCategories()
		{
			var category = await _unitOfWork.Category.GetAllAsync();
			return Ok(category);
		}
		[HttpGet("{id}", Name = "GetCategoryById")]
		public async Task<IActionResult> GetCategoryById(string id)
		{
			var category = await _unitOfWork.Category.GetByIdAsync(id);
			if (category == null)
			{
				return NotFound();
			}
			return Ok(category);
		}
		[HttpPost]
		[HttpPost]
		public async Task<IActionResult> CreateCategory([FromBody] Category category)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (!string.IsNullOrEmpty(category.Img))
			{
				var path = @"Images/Categories/";
				category.Img = await _imageService.SaveBase64ImageAsync(category.Img, path);
			}

			await _unitOfWork.Category.AddAsync(category);
			return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
		}



		[HttpPut("{id}")]
		public async Task<IActionResult> EditCategory(
				string id,
				[FromBody] Category category)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (!string.IsNullOrEmpty(category.Img))
			{
				await _imageService.DeleteFileAsync(category.Img);
				category.Img = null;
				var path = @"Images/Categories/";
				category.Img = await _imageService.SaveBase64ImageAsync(category.Img, path);
			}
			await _unitOfWork.Category.UpdateAsync(category);
			return Ok(category);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCategory(string id)
		{
			var category = await _unitOfWork.Category.GetByIdAsync(id);
			if (category == null)
			{
				return NotFound();
			}
			await _unitOfWork.Category.DeleteAsync(id);
			return StatusCode(StatusCodes.Status204NoContent, "Data Saved");
		}
		

	}
}
