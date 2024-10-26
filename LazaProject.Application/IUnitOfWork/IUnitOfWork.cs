﻿using LazaProject.Application.IRepository;
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
		IAuthRepo AuthRepo { get; }
		IEmailService EmailService { get; }
		
		Task<int> CompleteAsync();
	}
}