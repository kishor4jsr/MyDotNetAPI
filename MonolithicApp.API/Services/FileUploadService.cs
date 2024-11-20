using Microsoft.AspNetCore.Http;
using MonolithicApp.Data;
using MonolithicApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MonolithicApp.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileUploadService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<Guid?> UploadFileAsync(IFormFile file)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.Identity.Name;
                var fileId = Guid.NewGuid();
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, $"{fileId}_{file.FileName}");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileMetadata = new FileMetadata
                {
                    Id = fileId,
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    Size = file.Length,
                    UploadedAt = DateTime.UtcNow,
                    FilePath = filePath
                };

                _dbContext.FileMetadata.Add(fileMetadata);
                await _dbContext.SaveChangesAsync();

                return fileId;
            }
            catch
            {
                return null;
            }
        }


        public async Task<FileMetadata> GetFileAsync(Guid fileId)
        {
            var userId = _httpContextAccessor.HttpContext.User.Identity.Name;
            return await _dbContext.FileMetadata.FindAsync(fileId);
        }

        public async Task<bool> DeleteFileAsync(Guid fileId)
        {
            var userId = _httpContextAccessor.HttpContext.User.Identity.Name;
            var file = await _dbContext.FileMetadata.FindAsync(fileId);
            if (file == null) return false;

            File.Delete(file.FilePath);
            _dbContext.FileMetadata.Remove(file);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
