namespace ResumeExtractorAPI.Models.DTOs
{
    public class WorkExperience
    {
        public string? Company { get; set; }
        public string? Position { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public List<string>? Responsibilities { get; set; }
        public bool? IsCurrentlyWorkingHere { get; set; }
    }
}