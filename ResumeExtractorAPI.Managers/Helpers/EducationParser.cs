using System.Text.RegularExpressions;
using Entity=ResumeExtractorAPI.DataAccess.Entities;

namespace ResumeExtractorAPI.Managers.Helpers
{
    public class EducationParser
    {
        private static readonly HashSet<string> DegreeKeywords=new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            
            "Bachelor", "Master", "B.Tech", "M.Tech", "B.E", "M.E", "BSc", "MSc",
            "BCA", "MCA", "BS", "MS", "PhD", "Diploma"

        };
         private static readonly Regex InstitutionRegex=new(@"\b([A-Za-z][A-Za-z&\.\s]+(?:University|College|Institute|School|Academy|Engineering|Center|Centre|Polytechnic|Academia|Campus))\b",
            RegexOptions.IgnoreCase|RegexOptions.Compiled);
        private static readonly Regex GradeRegex=new(@"\b(?:Grade|GPA|CGPA|Percentage)[:\s]*(?<value>\d{1,2}(?:\.\d{1,2})?%?)\b",
            RegexOptions.IgnoreCase|RegexOptions.Compiled);
             public static string? DetectDegree(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;
                foreach(var degree in DegreeKeywords)
            {
                if(input.IndexOf(degree, StringComparison.OrdinalIgnoreCase)>=0)
                    return ExtractDegreePhase(degree,input);
            }
            return null;
        }
        private static string? ExtractDegreePhase(string  degree,string input)
        {
            var idx=input.IndexOf(degree,StringComparison.OrdinalIgnoreCase);
            if(idx<0) return null;
            var endIdx=input.IndexOfAny(new char[]{',',';','|','\n'},idx);
            if(endIdx<0) endIdx=input.Length;
            return input.Substring(idx,endIdx-idx).Trim();
        }
        public static string? DetectIntitution(string input)
        {
            var  match=InstitutionRegex.Match(input??string.Empty);
            if(match.Success) return match.Groups[1].Value.Trim();
            return null;
        }
         public static string? DetectGrade(string input)
        {
            var match=GradeRegex.Match(input?? string.Empty);
            if(!match.Success) return null;
            if(!string.IsNullOrWhiteSpace(match.Groups[1].Value)) //GPA/CGPA
                   return $"{match.Groups[1].Value.ToUpper()} {match.Groups[2].Value}";
            if(!string.IsNullOrWhiteSpace(match.Groups[3].Value)) //Percentage
                    return$"{match.Groups[3].Value}%";
            return null;
        }
        public static List<Entity.Education> ExtractEducation(string raw)
        {
            var section = CommonParsing.ExtractSection(raw, "Education|Academic|Qualifications");
            if (string.IsNullOrWhiteSpace(section)) return new List<Entity.Education>();
            var lines = section.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var educationList = new List<Entity.Education>();
            foreach(var line in lines)
            {
                var description=(line??string.Empty).Trim();
                if(string.IsNullOrWhiteSpace(description) || CommonParsing.IsLikelyHeader(description)) continue;
                var degree=DetectDegree(line);
                var institution=DetectIntitution(line);
                var (start,end)=CommonParsing.ExtractDates(line);
                var grade=DetectGrade(line);
                educationList.Add(new Entity.Education
                {
                    Id=Guid.NewGuid(),
                    Degree=degree,
                    Institution=institution,
                    StartDate=start,
                    EndDate=end,
                    Grade=grade,
                    Description=line
                });
            }
            return educationList;
        }
    }
}