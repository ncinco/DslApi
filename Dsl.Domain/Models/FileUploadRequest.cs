using Microsoft.AspNetCore.Http;

namespace Dsl.Domain.Models
{
    public class FileUploadRequest
    {
        public IFormFile FileDetails { get; set; }
    }
}