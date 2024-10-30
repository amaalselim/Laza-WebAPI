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
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//optionsBuilder.UseSqlServer("Server=db9471.public.databaseasp.net; Database=db9471; User Id=db9471; Password=Ed5_9+mDP4=g; Encrypt=False; MultipleActiveResultSets=True;");
			optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=LazaAPI;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
		}

	}
}
