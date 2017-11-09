namespace Nethereum.Worbooks.Tests
{
    public class WorbookTest
    {
        private readonly string _worbookFilePath;
        private readonly MardownHelper _mardownHelper = new MardownHelper();

        public WorbookTest(string worbookFilePath)
        {
            _worbookFilePath = worbookFilePath;
        }

        public string GetCodeSectionsFromWorkbook()
        {
            using (var file = System.IO.File.OpenText(_worbookFilePath))
            {
                //Given
                var workbookText = file.ReadToEnd();
                return _mardownHelper.ExtractCodeFromMarkdown(workbookText);
            }
        }
    }
}