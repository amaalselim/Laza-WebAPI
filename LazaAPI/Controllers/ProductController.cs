using AutoMapper;
using LazaProject.Application.IServices;
using LazaProject.Application.IUnitOfWork;
using LazaProject.Core.DTO_S;
using LazaProject.Core.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Security.Claims;

namespace LazaAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IImageService _imageService;
		private readonly IMapper _mapper;

		public ProductController(IUnitOfWork unitOfWork, IImageService imageService, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_imageService = imageService;
			_mapper = mapper;
		}
		[HttpGet]
		public async Task<IActionResult> ViewAllProduct()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _unitOfWork.Users.GetByIdAsync(userId);

			if (user == null)
			{
				return NotFound("User not found.");
			}
			var product = await _unitOfWork.Product.GetAllProAsync(user?.Gender);
			return Ok(product);
		}
		[HttpGet("category/{categoryId}")]
		public async Task<IActionResult> GetProductsByCategory(string categoryId)
		{
			var products = await _unitOfWork.Product.GetAllProductByCategoryIdAsync(categoryId);

			if (!products.Any())
			{
				return NotFound("No products found for the specified category.");
			}

			return Ok(products);
		}

		[HttpGet("{id}", Name = "GetProductById")]
		public async Task<IActionResult> GetProductById(string id)
		{
			var product = await _unitOfWork.Product.GetImagesByProductIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			return Ok(product);
		}
		[HttpPost]
		public async Task<IActionResult> CreateProduct([FromForm] ProductDTO productDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var product = _mapper.Map<Product>(productDTO);

			if (productDTO.Img != null)
			{
				var path = @"Images/Products/";
				product.Img = await _imageService.SaveImageAsync(productDTO.Img, path); 
			}

			await _unitOfWork.Product.AddAsync(product);

			if (productDTO.Images != null && productDTO.Images.Count > 0)
			{
				List<productImage> imagesToAdd = new List<productImage>();

				foreach (var smallImg in productDTO.Images)
				{
					if (smallImg != null)
					{
						var smallImgPath = @"Images/Products/Small/";
						var smallImgUrl = await _imageService.SaveImageAsync(smallImg, smallImgPath);

						imagesToAdd.Add(new productImage
						{
							Image = smallImgUrl,
							ProductId = product.Id
						});
					}
				}

				await _unitOfWork.ProductImage.AddRangeAsync(imagesToAdd);
			}

			return Ok(new
			{
				Message = "Product created successfully!",
				NewData = product.Images
			});
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> EditProduct(string id, [FromForm] ProductDTO productDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// Retrieve the product
			var product = await _unitOfWork.Product.GetByIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}

			var oldProductImg = product.Img; 

			_mapper.Map(productDTO, product);

			if (productDTO.Img != null)
			{
				if (!string.IsNullOrEmpty(oldProductImg))
				{
					await _imageService.DeleteFileAsync(oldProductImg);
				}
				var path = @"Images/Products/";
				product.Img = await _imageService.SaveImageAsync(productDTO.Img, path);
			}

			if (productDTO.Images != null && productDTO.Images.Count > 0)
			{
				List<productImage> imagesToAdd = new List<productImage>();

				foreach (var smallImg in productDTO.Images)
				{
					if (smallImg != null)
					{
						await _imageService.DeleteFileAsync(smallImg.ToString());
						productDTO.Img = null;
						var smallImgPath = @"Images/Products/Small/";
						var smallImgUrl = await _imageService.SaveImageAsync(smallImg, smallImgPath);

						imagesToAdd.Add(new productImage
						{
							Image = smallImgUrl,
							ProductId = product.Id
						});
					}
				}

				await _unitOfWork.ProductImage.AddRangeAsync(imagesToAdd);
			}

			await _unitOfWork.Product.UpdateAsync(product);

			return Ok(new
			{
				Message = "Product updated successfully!",
				NewData = product.Images
			});
		}



		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(string id)
		{
			var product = await _unitOfWork.Product.GetByIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			await _unitOfWork.Product.DeleteAsync(id);
			return Ok(new { message = "Product Deleted successfully!"});
		}

		[HttpGet("Search")]
		public async Task<IActionResult> SeachProducts([FromQuery]ProductSearchDTO productSearchDTO)
		{
			var products = await _unitOfWork.Product.SearchProductAsync(productSearchDTO.SearchTerm);
			if (!products.Any())
			{
				return NotFound("No products found matching the search term.");
			}

			return Ok(products);
		}
		[HttpGet("Sorted")]
		public async Task<IActionResult> GetProductsSortedByPrice()
		{
			var products = await _unitOfWork.Product.GetProductsSortedByPrice();
			if (products == null || !products.Any())
			{
				return NotFound("No products found.");
			}

			return Ok(products);
		}

	}
}
