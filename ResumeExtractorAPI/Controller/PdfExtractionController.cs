using Microsoft.AspNetCore.Mvc;
using ResumeExtractorAPI.Models.DTOs;
using ResumeExtractorAPI.Interfaces.IServices;
using AutoMapper;
namespace ResumeExtractorAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfExtractionController : ControllerBase
    {
        private readonly IPdfExtractionService _pdfExtractionService;
        public PdfExtractionController(IPdfExtractionService pdfExtractionService)
        {
            _pdfExtractionService = pdfExtractionService;
        }
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadResume([FromForm] ResumeUploadRequest request)
        {
            try
            {
                var result = await _pdfExtractionService.ProcessAsync(request.File);
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Result
                {
                    Success = false,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }
    }
}