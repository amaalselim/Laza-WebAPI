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


		public UnitOfWork(ApplicationDbContext context, IRepository<ApplicationUser> UserRepo, IAuthRepo authRepo,IEmailService emailService)
        {
            _context = context;
            Users = UserRepo;
			EmailService = emailService;
            AuthRepo = authRepo;
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
