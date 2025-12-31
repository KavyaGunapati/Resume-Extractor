using ResumeExtractorAPI.Interfaces.IManagers;
using ResumeExtractorAPI.Interfaces.IServices;
using ResumeExtractorAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
namespace ResumeExtractorAPI.Services
{
    public class PdfExtractionService : IPdfExtractionService
    {
        private readonly IPdfExtractionManager _manager;
        public PdfExtractionService(IPdfExtractionManager manager)
        {
            _manager=manager;
        }
        public async Task<Result> ProcessAsync(IFormFile file)
        {
            try
            {
                var result=await _manager.ProcessAsync(file);
                return result;
            }
            catch(Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error extracting text: {ex.Message}"
                };
            }
            
        }
         }
}