using ResumeExtractorAPI.Models.DTOs;
namespace ResumeExtractorAPI.Interfaces.IServices
{
    public interface IResumeService
    {
        Task<Result<List<ResumeResponse>>> SearchByKeywordAsync(string keyword);
    }
}