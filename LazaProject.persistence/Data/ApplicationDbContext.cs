using LazaProject.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=LazaAPI;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
		}
	}
}
