using System.ComponentModel.DataAnnotations;

namespace ResumeExtractorAPI.DataAccess.Entities
{
    public class WorkExperience
    {
        [Key]
        public Guid Id { get; set; }
        public string? Company { get; set; }
        public string? Position { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Description { get; set; }
        public string? YearsOfExperience { get; set; }
        public List<string>? Responsibilities { get; set; }
        public bool? IsCurrentlyWorkingHere { get; set; }

        public Guid ResumeResultId { get; set; }
        public ResumeResult? ResumeResult { get; set; }
    }
}