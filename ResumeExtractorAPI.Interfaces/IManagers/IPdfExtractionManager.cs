using Microsoft.AspNetCore.Http;
using ResumeExtractorAPI.Models;
using ResumeExtractorAPI.Models.DTOs;
namespace ResumeExtractorAPI.Interfaces.IManagers
{
    public interface IPdfExtractionManager
    {
        Task<Result> ProcessAsync(IFormFile file);
    }
}