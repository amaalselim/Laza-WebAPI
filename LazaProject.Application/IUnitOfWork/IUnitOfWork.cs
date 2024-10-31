using LazaProject.Application.IRepository;
using LazaProject.Application.IServices;
using LazaProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Application.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
	{

		IRepository<ApplicationUser> Users { get; }
		IRepository<Category> Category { get; }
		IProductRepository Product{ get; }
		IProductImageRepository ProductImage{ get; }
		IAuthRepo AuthRepo { get; }
		IEmailService EmailService { get; }
		IImageService ImageService { get; }
		ICartRepository cartRepository { get; }
		
		IWishListItemRepository WishListItemRepository { get; }
		IReviewRepository ReviewRepository { get; }
		Task<int> CompleteAsync();
	}
}
