using System.IO;

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

        public string GetCodeSectionsFromWorkbook()
        {
            using (var file = File.OpenText(_worbookFilePath))
            {
                //Given
                var workbookText = file.ReadToEnd();
                return _mardownHelper.ExtractCodeFromMarkdown(workbookText);
            }
        }
    }
}