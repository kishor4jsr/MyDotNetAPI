using Microsoft.AspNetCore.Http;
using MonolithicApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MonolithicApp.Services
{
    public interface IFileUploadService
    {
        Task<Guid?> UploadFileAsync(IFormFile file);
        Task<FileMetadata> GetFileAsync(Guid fileId);
        Task<bool> DeleteFileAsync(Guid fileId);
    }
}
