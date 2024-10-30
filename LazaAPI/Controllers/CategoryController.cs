using LazaProject.Application.IServices;
using LazaProject.Application.IUnitOfWork;
using LazaProject.Core.DTO_S; // تأكد من أن هذا هو المسار الصحيح لـ CategoryDTO
using LazaProject.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
			var categories = await _unitOfWork.Category.GetAllAsync();
			return Ok(categories);
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
		public async Task<IActionResult> CreateCategory([FromForm] CategoryDTO categoryDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var category = new Category
			{
				Name = categoryDTO.Name
			};

			if (categoryDTO.Img != null)
			{
				var path = @"Images/Categories/";
				category.Img= await _imageService.SaveImageAsync(categoryDTO.Img, path);
			}

			await _unitOfWork.Category.AddAsync(category);
			await _unitOfWork.CompleteAsync();

			return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> EditCategory(string id, [FromForm] CategoryDTO categoryDTO) // تعديل هنا لاستخدام CategoryDTO
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var existingCategory = await _unitOfWork.Category.GetByIdAsync(id);
			if (existingCategory == null)
			{
				return NotFound();
			}

			existingCategory.Name = categoryDTO.Name; 

			if (categoryDTO.Img != null && categoryDTO.Img.Length > 0)
			{
				await _imageService.DeleteFileAsync(categoryDTO.Img.ToString());
				categoryDTO.Img = null;
				var path = @"Images/Categories/";
				existingCategory.Img = await _imageService.SaveImageAsync(categoryDTO.Img, path);
			}

			await _unitOfWork.Category.UpdateAsync(existingCategory);
			await _unitOfWork.CompleteAsync();

			return Ok(existingCategory);
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
			await _unitOfWork.CompleteAsync();

			return Ok(new { message = "Category Deleted successfully!" });
		}
	}
}
