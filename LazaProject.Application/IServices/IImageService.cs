using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Application.IServices
{
    public interface IImageService
    {
		Task<string> SaveImageAsync(IFormFile Img, string folderPath);
		Task DeleteFileAsync(string File);
		Task<string> SaveBase64ImageAsync(string Img, string folderPath);
	}
}
