using LazaProject.Core.Enums;
using LazaProject.Core.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class ProductDTO
{
	public string Name { get; set; }
	public string Description { get; set; }
	public IFormFile Img { get; set; }
	public List<IFormFile> Images { get; set; } = new List<IFormFile>();
	public decimal Price { get; set; }
	[ForeignKey("Category")]
	public string CategoryId { get; set; }
}
