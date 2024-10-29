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
		Task<Product> GetImagesByProductIdAsync(string id);
		Task<IEnumerable<Product>> GetAllImagesByProducts();
		Task<IEnumerable<Product>> SearchProductAsync (string searchTerm);
		Task<IEnumerable<Product>> GetAllProductByCategoryIdAsync(string categoryid);
	}
}
