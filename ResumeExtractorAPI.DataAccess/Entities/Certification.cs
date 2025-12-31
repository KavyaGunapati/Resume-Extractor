using System.ComponentModel.DataAnnotations;

namespace ResumeExtractorAPI.DataAccess.Entities
{
    public class Certification
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Issuer { get; set; }
        public string? Date { get; set; }
        public string? Description { get; set; }
        public string? CredentialID { get; set; }
        public string? CredentialURL { get; set; }

        public Guid ResumeResultId { get; set; }
        public ResumeResult? ResumeResult { get; set; }
    }
}