using LazaProject.Core.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.persistence.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
        public ApplicationDbContext()
        {
            
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<ApplicationUser> applicationUsers{ get; set; }
		public DbSet<Category> categories{ get; set; }
		public DbSet<Product> products{ get; set; }
		public DbSet<productImage> productImages{ get; set; }
		public DbSet<WishListItem> wishListItems{ get; set; }
		public DbSet<Reviews> reviews{ get; set; }
		public DbSet<Cart> carts{ get; set; }
		public DbSet<CartItem> cartItems{ get; set; }
		public DbSet<AddressUser> address{ get; set; }
		public DbSet<Card> cards{ get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=db9584.public.databaseasp.net; Database=db9584; User Id=db9584; Password=8e+XzQ7=4-Tx; Encrypt=False; MultipleActiveResultSets=True;");
			//optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=LazaAPI;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
		}

	}
}
