using ECommercePlatform.Application.Contracts.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ECommercePlatform.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");

            Directory.CreateDirectory(folderPath);

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";

            var physicalPath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(physicalPath, FileMode.Create);
            await file.CopyToAsync(stream);

            string filePath = Path.Combine("images", fileName)
                .Replace("\\", "/");

            return filePath;
        }



        public async Task DeleteFilesAsync(List<string> filePaths)
        {
            foreach (var filePath in filePaths)
            {
                var physicalPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath);

                if (File.Exists(physicalPath))
                {
                    File.Delete(physicalPath);
                }
            }
        }
    }
}
