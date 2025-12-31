using ResumeExtractorAPI.Interfaces.IManagers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using IronOcr;
using ResumeExtractorAPI.Interfaces.IRepository;
using Entity=ResumeExtractorAPI.DataAccess.Entities;
using Azure.AI.DocumentIntelligence;
using ResumeExtractorAPI.Models.DTOs;
using System.Text.RegularExpressions;
namespace ResumeExtractorAPI.Managers
{
    public class PdfExtractionManager : IPdfExtractionManager
    {
        private readonly int _minlen;
        //private readonly DocumentIntelligenceClient _docClient;
        private readonly IBaseRepository<Entity.ResumeResult> _repository;
        private readonly IConfiguration _config;
        
        public PdfExtractionManager(IConfiguration cfg, IBaseRepository<Entity.ResumeResult> repository)
        {
            _minlen=int.TryParse(cfg["Extraction:MinAcceptableLength"],out var value)?value:400;
            _repository = repository;
            _config = cfg;
            
            //  IronPDF License
            var ironPdfKey = cfg["IronPDF:LicenseKey"];
            if (!string.IsNullOrWhiteSpace(ironPdfKey) )
            {
                IronPdf.License.LicenseKey = ironPdfKey;
                Console.WriteLine("IronPDF license loaded");
            }
            
            //  IronOCR License
            var ironOcrKey = cfg["IronOCR:LicenseKey"];
            if (!string.IsNullOrWhiteSpace(ironOcrKey) )
            {
                IronOcr.License.LicenseKey = ironOcrKey;
                Console.WriteLine("IronOCR license loaded");
            }
            
            //_docClient = new DocumentIntelligenceClient(
            //    new Uri(cfg["AzureDocumentIntelligence:Endpoint"]!),
            //    new Azure.AzureKeyCredential(cfg["AzureDocumentIntelligence:ApiKey"]!)
            //);
        }
       public async Task<Result> ProcessAsync(IFormFile file)
        {
            // 1) Save to temp file for IronPDF / IronOCR
            var tempFilePath=Path.GetTempFileName();
            using (var stream=File.Create(tempFilePath))
            {
                await file.CopyToAsync(stream);
            }
              // 2) IronPDF first
              string text=string.Empty;
              string source="IronPDF";
              try{
                var ironPDF=new PdfDocument(tempFilePath);
                text=ironPDF.ExtractAllText();
                Console.WriteLine($"IronPDF extracted: {text.Length} characters");
              }catch(Exception ex)
              {
                Console.WriteLine($"IronPDF error: {ex.Message}");
                text=string.Empty;
              }

              // 3) Extract text using IronOCR
           if(string.IsNullOrWhiteSpace(text)||text.Length<_minlen)
           {
                Console.WriteLine($"Text is empty or too short ({text.Length} chars), trying IronOCR...");
                try
                {
                    var ocr=new IronTesseract();
                    var input=new OcrInput();
                    input.LoadPdf(tempFilePath);
                    var result=ocr.Read(input);
                    text=result.Text;
                    source="IronOCR";
                    Console.WriteLine($"IronOCR extracted: {text.Length} characters");
                    
                }catch(Exception ex)
                {
                    Console.WriteLine($"IronOCR error: {ex.Message}");
                    text=string.Empty;
                }
           }
        //    //4) Azure Document Intelligence as fallback
        //       file.OpenReadStream().Position=0;
        //       var layoutOp=await _docClient.AnalyzeDocumentAsync(
        //         Azure.WaitUntil.Completed,"prebuilt-layout",
        //         BinaryData.FromStream(file.OpenReadStream()));
        //         var layoutResult=layoutOp.Value;
                // var lang=layoutResult.Languages?.FirstOrDefault()?.Locale;
                // 5) Map basic fields
                if(string.IsNullOrWhiteSpace(text))
                {
                    Console.WriteLine("WARNING: No text extracted from PDF!");
                }
                
                var resume=MapBasic(text);
                resume.Id=Guid.NewGuid();
                resume.Language="en";
                resume.SelectedSource=source;
                resume.RawText=text;
                
                if(resume.PersonalInfo!=null) resume.PersonalInfo.ResumeResultId=resume.Id;
                foreach(var e in resume.Education) e.ResumeResultId=resume.Id;
                foreach(var w in resume.WorkExperience) w.ResumeResultId=resume.Id;
                foreach(var s in resume.Skills) s.ResumeResultId=resume.Id;
                foreach(var p in resume.Projects) p.ResumeResultId=resume.Id;
                foreach(var c in resume.Certifications) c.ResumeResultId=resume.Id;
                foreach(var l in resume.Languages) l.ResumeResultId=resume.Id;
                foreach(var a in resume.Awards) a.ResumeResultId=resume.Id;
            try
            {
                await _repository.AddAsync(resume);
                
                await _repository.SaveChangesAsync();
                
                return new Result
                {
                    Success=true,
                    Message="Resume processed successfully",
                    Data=new ResumeResponse{ Id=resume.Id }
                };
            }
            catch(Exception ex)
            {
                
                return new Result
                {
                    Success=false,
                    Message=$"Error saving to database: {ex.Message}",
                    Data=null
                };
            }
            finally
            {
                // Clean up temp file
                if(File.Exists(tempFilePath))
                    File.Delete(tempFilePath);
            } 
        }
        // Map basic fields from extracted text
        private Entity.ResumeResult MapBasic(string text)
        {
            return new Entity.ResumeResult
            {
                PersonalInfo=ExtractPersonalInfo(text),
                Objective=ExtractSection(text,"Objective|Summary|Profile"),
                Skills=ExtractSkills(text),
                Education=Helpers.EducationParser.ExtractEducation(text),
                WorkExperience=Helpers.WorkExperienceParser.ExtractWorkExperience(text),
                Projects=ExtractProjects(text),
                Certifications=ExtractCertifications(text),
                Languages=ExtractLanguages(text),
                Awards=ExtractAwards(text)
            };
        }
        private Entity.PersonalInfo? ExtractPersonalInfo(string text){
            if(string.IsNullOrWhiteSpace(text))
                return null;
                var email=Regex.Match(text,@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}").Value;
                var phone=Regex.Match(text,@"\+?\d[\d\s\-()]{8,}").Value;
                var LinkedIn=Regex.Match(text,@"(https?:\/\/)?(www\.)?linkedin\.com\[^\s]+\/?").Value;
                var GitHub=Regex.Match(text,@"(https?:\/\/)?(www\.)?github\.com\[^\s]+\/?").Value;
                var nameLine=text.Split('\n').FirstOrDefault(l=>!string.IsNullOrWhiteSpace(l))??"Unknown";
                return new Entity.PersonalInfo
                {
                    Id=Guid.NewGuid(),
                    Name=nameLine.Trim(),
                    Email=string.IsNullOrWhiteSpace(email)? "unknown@example.com" : email,
                    Phone=string.IsNullOrWhiteSpace(phone)? null : phone,
                    LinkedIn=LinkedIn,
                    GitHub=GitHub,
                };
        
        }
        private string? ExtractSection(string raw,string titlePattern){
            var regex=new Regex($@"\b({titlePattern})\b",RegexOptions.IgnoreCase);
            var match=regex.Match(raw??string.Empty);
           return match.Success?match.Groups[2].Value.Trim():null;
        }
        private List<Entity.Skill> ExtractSkills(string raw){
            var skillSection= ExtractSection(raw,"Skills|Technical Skills|Key Skills|Skills & Expertise");
            if(string.IsNullOrWhiteSpace(skillSection))
                return new List<Entity.Skill>();
            var skills=skillSection.Split(new[]{',',';','\n'},StringSplitOptions.RemoveEmptyEntries)
            .Select(s=>new Entity.Skill{Id=Guid.NewGuid(),Name=s.Trim()})
            .ToList();
            return skills;
        }
        private List<Entity.Language> ExtractLanguages(string raw)
        {
            var langSection=ExtractSection(raw,"Languages|Language Proficiency");
            if(string.IsNullOrWhiteSpace(langSection))
                return new List<Entity.Language>();
        var languages=langSection.Split(new[]{',',';','\n'},StringSplitOptions.RemoveEmptyEntries)
        .Select(l=>new Entity.Language{Id=Guid.NewGuid(),Name=l.Trim()})
        .ToList();
        return languages;
        }
        
       
        private List<Entity.Project> ExtractProjects(string raw)
        {
            var projectSection= ExtractSection(raw,"Projects|Project Experience");
            if(string.IsNullOrWhiteSpace(projectSection))
                return new List<Entity.Project>();
            var projects=projectSection.Split(new[]{'\n'},StringSplitOptions.RemoveEmptyEntries)
            .Select(p=>new Entity.Project{Id=Guid.NewGuid(),Name=p.Trim()})
            .ToList();
            return projects;
        }
        private List<Entity.Certification> ExtractCertifications(string raw)
        {
           var CertificationSection= ExtractSection(raw,"Certifications|Licenses");
            if(string.IsNullOrWhiteSpace(CertificationSection))
                return new List<Entity.Certification>();
            var certifications=CertificationSection.Split(new[]{',',';','\n'},StringSplitOptions.RemoveEmptyEntries)
            .Select(c=>new Entity.Certification{Id=Guid.NewGuid(),Name=c.Trim()})
            .ToList();
            return certifications;
        }
        private List<Entity.Award> ExtractAwards(string raw)
        {
            var awardSection= ExtractSection(raw,"Awards|Honors|Achievements");
            if(string.IsNullOrWhiteSpace(awardSection))
                return new List<Entity.Award>();
            var awards=awardSection.Split(new[]{',',';','\n'},StringSplitOptions.RemoveEmptyEntries)
            .Select(a=>new Entity.Award{Id=Guid.NewGuid(),Title=a.Trim()})
            .ToList();
            return awards;
        }
    }
}