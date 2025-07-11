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
            // Определяем корень wwwroot
            var wwwRootPath = _env.WebRootPath;
            if (string.IsNullOrEmpty(wwwRootPath))
            {
                wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            // Строим путь: wwwroot/documents/projectName
            var projectFolder = Path.Combine(wwwRootPath, "documents", projectName);

            // Создаем папки, если их нет
            if (!Directory.Exists(projectFolder))
                Directory.CreateDirectory(projectFolder);

            // Добавляем расширение исходного файла
            var extension = Path.GetExtension(file.FileName);
            var filePath = Path.Combine(projectFolder, fileName + extension);

            // Сохраняем файл
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Возвращаем путь для доступа через браузер
            var relativePath = $"/documents/{projectName}/{fileName}{extension}";
            return relativePath;
        }
    }
}
