using ResumeExtractorAPI.DataAccess.Entities;
using ResumeExtractorAPI.Interfaces.IRepository;
using ResumeExtractorAPI.Interfaces.IManagers;
using ResumeExtractorAPI.Models.DTOs;
using AutoMapper;

namespace ResumeExtractorAPI.Managers
{
    public class ResumeManager:IResumeManager
    {
        private readonly IBaseRepository<ResumeResult> _resumeRepository;
        private readonly IMapper _mapper;
        
        private static readonly Dictionary<string,List<string>> SkillAliases = new(StringComparer.OrdinalIgnoreCase)
        {
            { "C#", new List<string> { "C#", "Csharp", "C sharp", "CS" } },
            { "JavaScript", new List<string> { "JavaScript", "JS", "java script" } },
            { ".NET", new List<string> { ".NET", "DotNet", "dot net" } },
            { "C++", new List<string> { "C++", "CPP", "C plus plus" } },
            { "Node.js", new List<string> { "Node.js", "Node", "Nodejs" } },
            { "Python", new List<string> { "Python", "py" } },
            { "SQL", new List<string> { "SQL", "T-SQL", "TSQL", "MySql", "MySQL" } },
            { "React", new List<string> { "React", "ReactJS", "React.js" } },
            { "Angular", new List<string> { "Angular", "AngularJS", "Angular.js" } },
            { "Vue", new List<string> { "Vue", "Vue.js", "Vuejs" } },
            { "AWS", new List<string> { "AWS", "Amazon", "Amazon Web Services" } },
            { "Azure", new List<string> { "Azure", "Microsoft Azure" } },
            { "Docker", new List<string> { "Docker", "Containers" } },
            { "Kubernetes", new List<string> { "Kubernetes", "K8s" } },
        };

        public ResumeManager(IBaseRepository<ResumeResult> resumeRepository, IMapper mapper)
        {
            _resumeRepository = resumeRepository;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Normalize skill name to match aliases
        /// </summary>
        private bool SkillMatches(string skillName, string keyword)
        {
            if (string.IsNullOrWhiteSpace(skillName) || string.IsNullOrWhiteSpace(keyword))
                return false;

            // Direct match
            if (skillName.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                return true;

            // Check aliases
            foreach(var aliasGroup in SkillAliases.Values)
            {
                if(aliasGroup.Any(alias=>alias.Equals(skillName,StringComparison.OrdinalIgnoreCase))||
                aliasGroup.Any(alias=>alias.Equals(keyword,StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<Result<List<ResumeResponse>>> SearchByKeywordAsync(string keyword)
        {
            try
            {
                var resumeResults = await _resumeRepository.GetAllAsync();
               var filteredResults=resumeResults.Where(r=>r.PersonalInfo!=null&&
                ((r.PersonalInfo.Name?.Contains(keyword,StringComparison.OrdinalIgnoreCase)==true) ||
                (r.PersonalInfo.Email?.Contains(keyword,StringComparison.OrdinalIgnoreCase)==true)||
                (r.PersonalInfo.Phone?.Contains(keyword,StringComparison.OrdinalIgnoreCase)==true)) ||
                (r.Skills!=null && r.Skills.Any(s=>SkillMatches(s.Name??string.Empty,keyword))) ||
                (r.WorkExperience!=null && r.WorkExperience.Any(w=>(
                    (w.Position?.Contains(keyword,StringComparison.OrdinalIgnoreCase)==true) ||
                    (w.Company?.Contains(keyword,StringComparison.OrdinalIgnoreCase)==true)||
                    (w.Description?.Contains(keyword,StringComparison.OrdinalIgnoreCase)==true))))||
                (r.Education!=null  && (r.Education.Any(e=>(
                    (e.Degree?.Contains(keyword,StringComparison.OrdinalIgnoreCase)==true)||
                    (e.Institution?.Contains(keyword,StringComparison.OrdinalIgnoreCase)==true)||
                    (e.Description?.Contains(keyword,StringComparison.OrdinalIgnoreCase)==true))))) ||
                (r.Projects!=null && r.Projects.Any(p=>(
                    (p.Description?.Contains(keyword,StringComparison.OrdinalIgnoreCase)==true)||
                    (p.Name?.Contains(keyword,StringComparison.OrdinalIgnoreCase)==true))))||
                (r.Languages!=null && r.Languages.Any(l=>l.Name?.Contains(keyword,StringComparison.OrdinalIgnoreCase)==true))).ToList();


                var resultList = _mapper.Map<List<ResumeResponse>>(filteredResults);
                return new Result<List<ResumeResponse>>
                {
                    Data = resultList,
                    Success = true,
                    Message = "Search completed successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result<List<ResumeResponse>>
                {
                    Data = null,
                    Success = false,
                    Message = $"An error occurred while searching: {ex.Message}"
                };
            }
        }
    }
}