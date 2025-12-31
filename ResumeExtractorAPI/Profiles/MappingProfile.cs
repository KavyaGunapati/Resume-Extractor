using AutoMapper;
using DTO=ResumeExtractorAPI.Models.DTOs;
using Entity=ResumeExtractorAPI.DataAccess.Entities;

namespace ResumeExtractorAPI.Profiles
{    
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Resume 
            CreateMap<DTO.ResumeCreateRequest, Entity.ResumeResult>().ReverseMap();
            CreateMap<Entity.ResumeResult, DTO.ResumeResponse>()
                .ForMember(dest => dest.PersonalInfo, opt => opt.MapFrom(src => src.PersonalInfo))
                .ForMember(dest => dest.Education, opt => opt.MapFrom(src => src.Education.ToList()))
                .ForMember(dest => dest.WorkExperience, opt => opt.MapFrom(src => src.WorkExperience.ToList()))
                .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skills.Select(s => s.Name).ToList()))
                .ForMember(dest => dest.Projects, opt => opt.MapFrom(src => src.Projects.ToList()))
                .ForMember(dest => dest.Certifications, opt => opt.MapFrom(src => src.Certifications.ToList()))
                .ForMember(dest => dest.Languages, opt => opt.MapFrom(src => src.Languages.ToList()))
                .ForMember(dest => dest.Awards, opt => opt.MapFrom(src => src.Awards.ToList()));

            // Personal Info
            CreateMap<DTO.PersonalInfo, Entity.PersonalInfo>().ReverseMap();
            CreateMap<Entity.PersonalInfo, DTO.PersonalInfo>();
            
            // Education
            CreateMap<DTO.Education, Entity.Education>().ReverseMap();
            CreateMap<Entity.Education, DTO.Education>();

            // Work Experience
            CreateMap<DTO.WorkExperience, Entity.WorkExperience>().ReverseMap();
            CreateMap<Entity.WorkExperience, DTO.WorkExperience>();

            // Skills
            CreateMap<DTO.Skill, Entity.Skill>().ReverseMap();
            CreateMap<Entity.Skill, DTO.Skill>();

            // Projects
            CreateMap<DTO.Project, Entity.Project>().ReverseMap();
            CreateMap<Entity.Project, DTO.Project>();

            // Certifications
            CreateMap<DTO.Certification, Entity.Certification>().ReverseMap();
            CreateMap<Entity.Certification, DTO.Certification>();

            // Languages
            CreateMap<DTO.Language, Entity.Language>().ReverseMap();
            CreateMap<Entity.Language, DTO.Language>();

            // Awards
            CreateMap<DTO.Award, Entity.Award>().ReverseMap();
            CreateMap<Entity.Award, DTO.Award>();
        }
    }
}

