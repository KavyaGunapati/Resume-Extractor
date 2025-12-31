using System.ComponentModel.DataAnnotations;

namespace ResumeExtractorAPI.DataAccess.Entities
{
    public class Project
{
    [Key]
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Role { get; set; }
    public List<string>? Technologies { get; set; }
    
        public Guid ResumeResultId { get; set; }
        public ResumeResult? ResumeResult { get; set; }
}

}