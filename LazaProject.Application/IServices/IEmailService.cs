using LazaProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Application.IServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email,string userName, string Subject, string message);
		Task<bool> SendOrderConfirmationEmailAsync(string email, string userName, Cart cart, AddressUser billingAddress, Card paymentCard);

	}
}
