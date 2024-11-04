using AutoMapper;
using LazaProject.Application.IRepository;
using LazaProject.Core.DTO_S;
using LazaProject.Core.Models;
using LazaProject.persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.persistence.Repository
{
	public class AddressRepository : IAddressRepository
	{
		private readonly ApplicationDbContext _context;

		public AddressRepository(ApplicationDbContext context)
        {
			_context = context;
		}
		public async Task AddAsync(AddressUser addressUser)
		{
			await _context.address.AddAsync(addressUser);
			await _context.SaveChangesAsync();
		}
	}
}
