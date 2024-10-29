using LazaProject.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class ProductDTO
{
	public string Name { get; set; }
	public string Description { get; set; }
	public string Img { get; set; }
	public decimal Price { get; set; }
	[ForeignKey("Category")]
	public string CategoryId { get; set; }
	[JsonIgnore]
	public Category? Category { get; set; }
	public List<string> Images { get; set; }
}
