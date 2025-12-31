using System.ComponentModel.DataAnnotations;

namespace ResumeExtractorAPI.DataAccess.Entities
{
   public class Award
{
    [Key]
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Issuer { get; set; }
    public string? Date { get; set; }

        public Guid ResumeResultId { get; set; }
        public ResumeResult? ResumeResult { get; set; }

}

}