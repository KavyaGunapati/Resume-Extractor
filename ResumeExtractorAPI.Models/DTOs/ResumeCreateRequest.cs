namespace ResumeExtractorAPI.Models.DTOs
{
    public class ResumeCreateRequest
    {
        public PersonalInfo? PersonalInfo { get; set; }

        public string? Objective { get; set; }

        // Stored in DB (JSON or Skill table – your choice)
        public List<string>? Skills { get; set; }

        public List<Education>? Education { get; set; }
        public List<WorkExperience>? WorkExperience { get; set; }
        public List<Project>? Projects { get; set; }
        public List<Certification>? Certifications { get; set; }
        public List<Language>? Languages { get; set; }
        public List<Award>? Awards { get; set; }
    }
}