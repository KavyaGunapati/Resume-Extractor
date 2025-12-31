using System.ComponentModel.DataAnnotations;

namespace ResumeExtractorAPI.DataAccess.Entities
{
    public class PersonalInfo
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }=null!;
        public string Email { get; set; }=null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? LinkedIn { get; set; }
        public string? GitHub { get; set; }

        // Foreign Key and Navigation Property
        public Guid ResumeResultId { get; set; }
        public ResumeResult? ResumeResult { get; set; }
    }

}