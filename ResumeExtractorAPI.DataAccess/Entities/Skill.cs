namespace ResumeExtractorAPI.DataAccess.Entities
{
    public class Skill
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public Guid ResumeResultId { get; set; }
        public ResumeResult? ResumeResult { get; set; }
    }
}