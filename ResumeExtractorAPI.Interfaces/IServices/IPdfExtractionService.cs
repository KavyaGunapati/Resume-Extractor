using Microsoft.AspNetCore.Http;
using ResumeExtractorAPI.Models.DTOs;
namespace ResumeExtractorAPI.Interfaces.IServices
{
    public interface IPdfExtractionService
    {
        Task<Result> ProcessAsync(IFormFile file);
    }
}