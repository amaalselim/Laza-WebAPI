using LazaProject.Application.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.persistence.Services
{
	public class ImageService : IImageService
	{
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ImageService(IWebHostEnvironment webHostEnvironment)
        {
			_webHostEnvironment = webHostEnvironment;
		}
        public async Task DeleteFileAsync(string File)
		{
			if (File != null)
			{
				var RootPath = _webHostEnvironment.WebRootPath;
				var oldFile = Path.Combine(RootPath, File);

				if (System.IO.File.Exists(oldFile))
				{
					System.IO.File.Delete(oldFile);
				}
			}
		}
		public async Task<string> SaveBase64ImageAsync(string base64Image, string folderName)
		{
			if (base64Image.StartsWith("data:image/jpeg;base64,"))
			{
				base64Image = base64Image.Substring("data:image/jpeg;base64,".Length);
			}
			else if (base64Image.StartsWith("data:image/png;base64,")) 
			{
				base64Image = base64Image.Substring("data:image/png;base64,".Length);
			}

			if (string.IsNullOrEmpty(base64Image))
			{
				throw new FormatException("The provided Base64 string is empty after removing the header.");
			}

			try
			{
				byte[] imageBytes = Convert.FromBase64String(base64Image);

				string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, folderName);

				if (!Directory.Exists(folderPath))
				{
					Directory.CreateDirectory(folderPath);
				}

				string fileName = $"{Guid.NewGuid()}.jpg"; 
				string filePath = Path.Combine(folderPath, fileName);

				await File.WriteAllBytesAsync(filePath, imageBytes);

				return filePath;
			}
			catch (FormatException ex)
			{

				throw new Exception("Invalid Base64 string provided.", ex);
			}
			catch (Exception ex)
			{

				throw new Exception("Error saving image.", ex);
			}
		}

		public async Task<string> SaveImageAsync(IFormFile Img, string folderPath)
		{
			if (Img == null || Img.Length == 0)
			{
				return null;
			}

			string fileName = Guid.NewGuid().ToString();
			string extension = Path.GetExtension(Img.FileName);
			string filePath = Path.Combine(_webHostEnvironment.WebRootPath, folderPath, fileName + extension);

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await Img.CopyToAsync(fileStream);
			}

			return Path.Combine(folderPath, fileName + extension);
		}

	}
}
