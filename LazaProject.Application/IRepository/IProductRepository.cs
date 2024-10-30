using LazaProject.Core.DTO_S;
using LazaProject.Core.Enums;
using LazaProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Application.IRepository
{
	public interface IProductRepository : IRepository<Product>
	{
		Task<ProductDetailsDTO> GetImagesByProductIdAsync(string id);
		Task<IEnumerable<Product>> GetAllImagesByProducts();
		Task<IEnumerable<Product>> SearchProductAsync (string searchTerm);
		Task<IEnumerable<Product>> GetAllProductByCategoryIdAsync(string categoryid,Gender? gender);
		Task<IEnumerable<Product>> GetProductsSortedByPrice();
		Task<IEnumerable<Product>> GetAllProAsync(Gender? gender);
	}
}
