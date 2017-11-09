using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Nethereum.Worbooks.Tests
{
    public class MardownHelper
    {
        public string ExtractCodeFromMarkdown(string text)
        {
            var stringBuilder = new StringBuilder();
            var matches = Regex.Matches(text, @"```csharp([\s\S]+?)```", RegexOptions.Multiline);

            foreach (var match in matches.ToArray())
            {
                var textMatched = match.Groups[1].Value;
                stringBuilder.Append(textMatched);
            }

            return stringBuilder.ToString();
        }
    }
}