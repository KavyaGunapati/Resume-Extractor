using System.Text.RegularExpressions;
using Entity=ResumeExtractorAPI.DataAccess.Entities;
namespace ResumeExtractorAPI.Managers.Helpers
{
    public class WorkExperienceParser
    {
        private static readonly Regex CompanyRegex=new(@"\b([A-Za-z][A-Za-z&\.\s]+(?:Inc|LLC|Ltd|Corporation|Corp|Company|Co|Technologies|Systems|Solutions|Enterprises|Group|Holdings|International|Global))\b",
            RegexOptions.IgnoreCase|RegexOptions.Compiled);  
        private static readonly Regex PositionRegex=new(@"\b([A-Za-z][A-Za-z&\.\s]+(?:Engineer|Developer|Manager|Director|Analyst|Consultant|Specialist|Coordinator|Administrator|Architect|Designer|Scientist|Technician|Supervisor|Executive|Officer|Lead|Head|Intern))\b",
            RegexOptions.IgnoreCase|RegexOptions.Compiled);

        public static string? DetectCompany(string input)
        {
            var match=CompanyRegex.Match(input??string.Empty);
            if(match.Success) return match.Groups[1].Value.Trim();
            return null;
        }
        public static string? DetectPosition(string input)
        {
            var match=PositionRegex.Match(input??string.Empty);
            if(match.Success) return match.Groups[1].Value.Trim();
            return null;
        }
        public static List<Entity.WorkExperience> ExtractWorkExperience(string raw)
        {
            var section = CommonParsing.ExtractSection(raw, "Experience|Work Experience|Employment|Professional Experience");
            if (string.IsNullOrWhiteSpace(section))
                return new List<Entity.WorkExperience>();
           var experience=section.Split('\n').Where(l=>!string.IsNullOrWhiteSpace(l))
              .Select(line=>{
                 var (startDate,endDate)=CommonParsing.ExtractDates(line);
                 var yearsOfExp="";
                 if(!string.IsNullOrWhiteSpace(startDate))
                 {
                   DateTime startDt;
                   if(DateTime.TryParse(startDate,out startDt))
                      {
                        var endDt=DateTime.Now;
                          if (!string.IsNullOrWhiteSpace(endDate) && !CommonParsing.IsCurrentEnd(endDate))
                          {
                              DateTime parseEnd;
                              if(DateTime.TryParse(endDate,out parseEnd))
                              {
                                  endDt = parseEnd;
                              }
                          }
                          var span=endDt - startDt;
                          var years=(span.Days/365).ToString();
                          var months=(span.Days%365)/30;
                            if(months>0)
                            {
                                yearsOfExp=$"{years} years {months} months";
                            }
                            else
                            {
                                yearsOfExp=$"{years} years";
                            }
                      }
                 }
                 return new Entity.WorkExperience
                 {
                      Id=Guid.NewGuid(),
                      Position=DetectPosition(line),
                      Company=DetectCompany(line),
                      YearsOfExperience=yearsOfExp,
                      StartDate=startDate,
                      EndDate=endDate,
                      IsCurrentlyWorkingHere=CommonParsing.IsCurrentEnd(endDate),
                      Description=line.Trim()
                 };
              }).ToList();
              return experience;
        }
    }
}