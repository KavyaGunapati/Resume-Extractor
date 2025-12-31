using System.Text.RegularExpressions;

namespace ResumeExtractorAPI.Managers.Helpers
{
    public static class CommonParsing
    {
        private static readonly Regex DateRangeRegex=new(@"\b(?<start>(?:Jan|Feb|
        Mar|Apr|May|Jun|Jul|Aug|Sep|Sept|Oct|Nov|Dec|January|February|
        March|April|May|June|July|August|September|October|November|December|
        \d{1,2}[/\-]\d{2,4}))\s*(?:-|–|—|to)\s*(?<end>(?:Present|Current|Jan|Feb
        |Mar|Apr|May|Jun|Jul|Aug|Sep|Sept|Oct|Nov|Dec|January|February|
        March|April|May|June|July|August|September|October|November|December
        |\d{1,2}[/\-]\d{2,4}))\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex SingleDateRegex=new(@"\b(?<date>(?:Jan|Feb|Mar
        |Apr|May|Jun|Jul|Aug|Sep|Sept|Oct|Nov|Dec|January|
        February|March|April|May|June|July|August|September|
        October|November|December|\d{1,2}[/\-]\d{2,4}))\b",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static (string? start, string? end) ExtractDates(string input)
        {
            var dateRangeMatch=DateRangeRegex.Match(input??string.Empty);
            if (dateRangeMatch.Success)
            {
                var start=dateRangeMatch.Groups["start"].Value.Trim();
                var end=dateRangeMatch.Groups["end"].Value.Trim();
                return (start,end);
            }
            var singleDateMatch=SingleDateRegex.Matches(input??string.Empty)
            .Select(m=>m.Groups["date"].Value.Trim()).ToList();
            if(singleDateMatch.Count==1) return (singleDateMatch[0],null);
            if(singleDateMatch.Count>=2) return (singleDateMatch[0],singleDateMatch[1]);
            return (null,null);
            
        }
        public static bool IsCurrentEnd(string? end)
        {
            return !string.IsNullOrWhiteSpace(end) && (end.Equals("Present",StringComparison.OrdinalIgnoreCase)
            ||end.Equals("Current",StringComparison.OrdinalIgnoreCase)
            ||end.Equals("Till Date",StringComparison.OrdinalIgnoreCase)
            ||end.Equals("To Date",StringComparison.OrdinalIgnoreCase)
            ||end.Equals("Now",StringComparison.OrdinalIgnoreCase));

        }
        // header check
        public static bool IsLikelyHeader(string text)
        {
            if(string.IsNullOrWhiteSpace(text)) return false;
            var t=(text??string.Empty).Trim();
            return t.Length<= 3 || t.EndsWith(":", StringComparison.Ordinal);
        }
        
        public static string? ExtractSection(string raw, string titlePattern, int maxChars = 2000)
        {
            if(string.IsNullOrWhiteSpace(raw)) return null;
            var lines=raw.Split(new[] {'\n','\r'},StringSplitOptions.None).ToList();
            var startIndex=-1;
            var regexMatch=new Regex(titlePattern,RegexOptions.IgnoreCase);
            for(int i = 0; i < lines.Count; i++)
            {
                if (regexMatch.IsMatch(lines[i]))
                {
                    startIndex = i;
                    break;
                }
            }

            if(startIndex<0) return null;
            var section=new List<string>();
            for(int j=startIndex+1;j<lines.Count;j++){
                var line=lines[j];
                if(IsLikelyHeader(line)) break;
                section.Add(line);
            }
            var result=string.Join("\n",section).Trim();
            if(result.Length>maxChars)
                 result=result.Substring(0,maxChars).Trim();
            return string.IsNullOrWhiteSpace(result)?null:result;
        }
    }
}