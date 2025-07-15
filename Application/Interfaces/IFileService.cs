using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFileService
    {
        public  Task<string> SaveFileAsync(IFormFile file, string fileName, string projectName);
        // need to implement delete file method
    }
}
