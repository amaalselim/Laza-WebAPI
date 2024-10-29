using AutoMapper;
using LazaProject.Application.IServices;
using LazaProject.Application.IUnitOfWork;
using LazaProject.Core.DTO_S;
using LazaProject.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

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
			var product = await _unitOfWork.Product.GetAllAsync();
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
		public async Task<IActionResult> CreateProduct([FromBody] ProductDTO productDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var product = _mapper.Map<Product>(productDTO);

			if (!string.IsNullOrEmpty(product.Img))
			{
				var path = @"Images/Products/";
				product.Img = await _imageService.SaveBase64ImageAsync(product.Img, path);
			}

			await _unitOfWork.Product.AddAsync(product);

			if (product.Images != null && product.Images.Count > 0)
			{
				List<productImage> imagesToAdd = new List<productImage>();

				foreach (var smallImg in product.Images)
				{
					if (!string.IsNullOrEmpty(smallImg.Image))
					{
						var smallImgPath = @"Images/Products/Small/";
						var smallImgUrl = await _imageService.SaveBase64ImageAsync(smallImg.Image, smallImgPath);

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
				Message = "Product Created successfully!",
				NewData = product.Images.Where(img => !img.Image.StartsWith("data:image"))
			});
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> EditProduct(
		string id,
		[FromBody] ProductDTO productDTO)
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

			// Store old product data
			var oldProductData = _mapper.Map<ProductDTO>(product);

			// Map new product data
			_mapper.Map(productDTO, product);

			// Image handling
			if (!string.IsNullOrEmpty(productDTO.Img))
			{
				if (!string.IsNullOrEmpty(oldProductData.Img))
				{
					await _imageService.DeleteFileAsync(oldProductData.Img);
				}
				var path = @"Images/Products/";
				product.Img = await _imageService.SaveBase64ImageAsync(productDTO.Img, path);
			}
			if (product.Images != null && product.Images.Count > 0)
			{
				List<productImage> imagesToAdd = new List<productImage>();

				foreach (var smallImg in product.Images)
				{
					if (!string.IsNullOrEmpty(smallImg.Image))
					{
						var smallImgPath = @"Images/Products/Small/";
						var smallImgUrl = await _imageService.SaveBase64ImageAsync(smallImg.Image, smallImgPath);

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
				NewData = product.Images.Where(img => !img.Image.StartsWith("data:image"))
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

	}
}
