using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonolithicApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonolithicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IFileUploadService _fileService;

        public FileController(IFileUploadService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
            if (file == null || file.Length == 0)
            return BadRequest("File is not provided or is empty.");

        var result = await _fileService.UploadFileAsync(file);

        if (result == null)
            return StatusCode(500, "An error occurred while uploading the file.");

        return Ok(new { Message = "File uploaded successfully", FileId = result });
    }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile(Guid id)
        {
            var file = await _fileService.GetFileAsync(id);
            if (file == null) return NotFound();

            return PhysicalFile(file.FilePath, file.ContentType, file.FileName);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(Guid id)
        {
            var result = await _fileService.DeleteFileAsync(id);
            if (!result) return NotFound();

            return   Ok(new { Message = "File deleted successfully" }); ;
        }
    }
}
