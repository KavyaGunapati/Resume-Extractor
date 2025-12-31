using Microsoft.AspNetCore.Http;
namespace ResumeExtractorAPI.Models.DTOs
{
    public class ResumeUploadRequest
    {
        public IFormFile File { get; set; } = null!;
    }
}