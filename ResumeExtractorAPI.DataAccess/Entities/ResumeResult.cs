using System.ComponentModel.DataAnnotations;

namespace ResumeExtractorAPI.DataAccess.Entities
{
     public class ResumeResult
    {
    [Key]
    public Guid Id { get; set; }

    public PersonalInfo? PersonalInfo { get; set; }
    public string? RawText { get; set; }
    public string? Language { get; set; }
    public string? SelectedSource { get; set; }
    public string? Objective { get; set; }

    public ICollection<Skill> Skills { get; set; } = new List<Skill>();
    public ICollection<Education> Education { get; set; } = new List<Education>();
    public ICollection<WorkExperience> WorkExperience { get; set; } = new List<WorkExperience>();
    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<Certification> Certifications { get; set; } = new List<Certification>();
    public ICollection<Language> Languages { get; set; } = new List<Language>();
    public ICollection<Award> Awards { get; set; } = new List<Award>();
}

}
