using ResumeExtractorAPI.Interfaces.IServices;
using ResumeExtractorAPI.Interfaces.IManagers;
using ResumeExtractorAPI.Models.DTOs;
namespace ResumeExtractorAPI.Services   
{
    public class ResumeService:IResumeService
    {
        private readonly IResumeManager _manager;
        public ResumeService(IResumeManager manager)
        {
            _manager=manager;
        }
        public async Task<Result<List<ResumeResponse>>> SearchByKeywordAsync(string keyword)
        {
            Result<List<ResumeResponse>> result=new Result<List<ResumeResponse>>();
            try
            {
                result=await _manager.SearchByKeywordAsync(keyword);
                
            }
            catch(Exception ex)
            {
                result.Success=false;
                result.Message=$"Error searching resumes: {ex.Message}";
            }
            return result;
        }
    }
}