using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Application.IServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string Subject, string message);
    }
}
