using System.IO;
using System;
using System.Text;

namespace Nethereum.Worbooks.Tests
{
    public class WorbookTest
    {
        private readonly MardownHelper _mardownHelper = new MardownHelper();
        private readonly string _worbookFilePath;

        public WorbookTest(string worbookFilePath)
        {
            _worbookFilePath = worbookFilePath;
        }
        public string LoadCodeSection(string filePath)
        {
            using (var file = File.OpenText(filePath))
            {
                return file.ReadToEnd();
            }
        } 

        public string GetCodeSectionsFromWorkbook()
        {
            using (var file = File.OpenText(_worbookFilePath))
            {
                var workbookText = file.ReadToEnd();
                return _mardownHelper.ExtractCodeFromMarkdown(workbookText);
            }
        }

        public string RemoveLoadSections(string code)
        {
            var lines = code.Split('\n');
            var sb = new StringBuilder();
            foreach (var line in lines)
            {
                if (!line.StartsWith("#load")&&(!line.StartsWith("#r"))&&(!line.StartsWith("using")))
                {
                    sb.AppendLine(line);
                }
            }
            return sb.ToString();
        }
        
        public string ExtractUsingStatements(string code)
        {
            var lines = code.Split('\n');
            var sb = new StringBuilder();
            foreach (var line in lines)
            {
                if (line.Trim().StartsWith("using"))
                {
                    sb.AppendLine(line);
                }
            }
            return sb.ToString();
        }
        
        public string ExtractRStatements(string code)
        {
            var lines = code.Split('\n');
            var sb = new StringBuilder();
            foreach (var line in lines)
            {
                if (line.Trim().StartsWith("#r"))
                {
                    sb.AppendLine(line);
                }
            }
            return sb.ToString();
        }
        public string CorrectKeystorePath(string code)
        {
            code = code.Replace(".\\testchain\\clique\\devChain\\keystore", "");
            return code;
        }
    }
}
