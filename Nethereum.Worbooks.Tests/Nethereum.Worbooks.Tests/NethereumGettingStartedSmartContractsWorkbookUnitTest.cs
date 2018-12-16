using Microsoft.CodeAnalysis.CSharp.Scripting;
using Xunit;

namespace Nethereum.Worbooks.Tests
{
    public class NethereumGettingStartedSmartContractsWorkbookTest : WorbookTest
    {
        public NethereumGettingStartedSmartContractsWorkbookTest() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = "nethereum-smartcontrats-gettingstarted.workbook";

        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            //When
            var state = await CSharpScript.RunAsync(code);
             state = await state.ContinueWithAsync("return (transferNonce, balance);");
            var returnValue = (dynamic)state.ReturnValue;
            //Then
            Assert.Equal(2, returnValue.Item1);
            Assert.Equal(100000, returnValue.Item2);
        }
    }
}