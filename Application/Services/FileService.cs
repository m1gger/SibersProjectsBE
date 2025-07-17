using Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string fileName, string projectName)
        {
            var wwwRootPath = _env.WebRootPath;
            if (string.IsNullOrEmpty(wwwRootPath))
            {
                wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var projectFolder = Path.Combine(wwwRootPath, "documents", projectName);

            if (!Directory.Exists(projectFolder))
                Directory.CreateDirectory(projectFolder);

            var filePath = Path.Combine(projectFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Возвращаем путь для доступа через браузер
            var relativePath = $"/documents/{projectName}/{fileName}";
            return relativePath;
        }
    }
}
