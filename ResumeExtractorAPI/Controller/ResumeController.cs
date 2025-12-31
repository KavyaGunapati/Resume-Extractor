using Microsoft.AspNetCore.Mvc;
using ResumeExtractorAPI.Interfaces.IServices;

namespace ResumeExtractorAPI.Controller{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController: ControllerBase
    {

        private readonly IResumeService _resumeService;
        public ResumeController(IResumeService resumeService)
        {
            _resumeService = resumeService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByKeyword([FromQuery] string keyword)
        {
            var result = await _resumeService.SearchByKeywordAsync(keyword);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}