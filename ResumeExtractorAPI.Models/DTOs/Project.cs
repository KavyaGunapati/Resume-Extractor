namespace ResumeExtractorAPI.Models.DTOs
{
    public class Project
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Role { get; set; }
        public List<string>? Technologies { get; set; }
    }
}