using System.ComponentModel.DataAnnotations;

namespace ResumeExtractorAPI.DataAccess.Entities
{
    public class Education
    {
        [Key]
        public Guid Id { get; set; }
        public string? Degree { get; set; }
        public string? Institution { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Grade { get; set; }
        public string? Description { get; set; }
        
        public Guid ResumeResultId { get; set; }
        public ResumeResult? ResumeResult { get; set; }
    }
}
