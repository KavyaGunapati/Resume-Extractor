using System.ComponentModel.DataAnnotations;

namespace ResumeExtractorAPI.DataAccess.Entities
{
    public class Language
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Proficiency { get; set; }
        
        public Guid ResumeResultId { get; set; }
        public ResumeResult? ResumeResult { get; set; }
    }

}