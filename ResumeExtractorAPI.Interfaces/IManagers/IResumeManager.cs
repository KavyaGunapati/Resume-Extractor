using ResumeExtractorAPI.Models.DTOs;

namespace ResumeExtractorAPI.Interfaces.IManagers
{
    public interface IResumeManager
    {
        Task<Result<List<ResumeResponse>>> SearchByKeywordAsync(string keyword);

    }
}