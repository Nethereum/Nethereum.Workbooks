using Microsoft.CodeAnalysis.CSharp.Scripting;
using Xunit;

namespace Nethereum.Worbooks.Tests
{
    public class NethereumEventDtosGetAllChangesWorkbookTest : WorbookTest
    {
        public NethereumEventDtosGetAllChangesWorkbookTest() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = "nethereum-eventdtos-getallchanges.workbook";

        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            //When
            var state = await CSharpScript.RunAsync(code);
            state = await state.ContinueWithAsync("return (gas, logs2);");
            var returnValue = (dynamic)state.ReturnValue;
            //Then
            Assert.Equal(25535, returnValue.Item1.Value);
            Assert.Equal(1, returnValue.Item2.Count);

        }
    }
}