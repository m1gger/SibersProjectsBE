using Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text.RegularExpressions;
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

            projectName = MakeSafeName(projectName);

            var originalExtension = Path.GetExtension(file.FileName); 
            var safeFileName = MakeSafeName(Path.GetFileNameWithoutExtension(fileName));

            if (safeFileName.EndsWith(originalExtension.TrimStart('.'), StringComparison.OrdinalIgnoreCase))
            {
                safeFileName = safeFileName.Substring(0, safeFileName.Length - originalExtension.TrimStart('.').Length);
            }

            var finalFileName = $"{safeFileName}_{Guid.NewGuid()}{originalExtension}";

            var projectFolder = Path.Combine(wwwRootPath, "documents", projectName);
            if (!Directory.Exists(projectFolder))
                Directory.CreateDirectory(projectFolder);

            var filePath = Path.Combine(projectFolder, finalFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = $"/documents/{projectName}/{finalFileName}";
            return relativePath;
        }


        private string MakeSafeName(string name)
        {
            var safe = Regex.Replace(name, @"[^a-zA-Z0-9_\-]", "_");
            return safe;
        }
    }
}
