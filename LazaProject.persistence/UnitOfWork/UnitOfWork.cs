using LazaProject.Application.IRepository;
using LazaProject.Application.IServices;
using LazaProject.Application.IUnitOfWork;
using LazaProject.Core.Models;
using LazaProject.persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IRepository<ApplicationUser> Users { get; private set; }
        
        public IAuthRepo AuthRepo { get; private set; }
        public IEmailService EmailService { get; private set; }

		public IRepository<Category> Category { get; private set; }

		public IProductRepository Product { get; private set; }

		public IImageService ImageService { get; private set; }

		public IProductImageRepository ProductImage { get; private set; }

		public IWishListItemRepository WishListItemRepository { get; private set; }

		public IReviewRepository ReviewRepository { get; private set; }

		public UnitOfWork(ApplicationDbContext context, IRepository<ApplicationUser> UserRepo, IAuthRepo authRepo,IEmailService emailService,IRepository<Category> cat , IProductRepository pro,IImageService imageService,IProductImageRepository productimg,IWishListItemRepository wishListItem ,IReviewRepository reviewRepository)
        {
            _context = context;
            Users = UserRepo;
			EmailService = emailService;
            AuthRepo = authRepo;
            Category = cat;
            Product = pro;
            ImageService = imageService;
            ProductImage = productimg;
            WishListItemRepository = wishListItem;
            ReviewRepository = reviewRepository;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
