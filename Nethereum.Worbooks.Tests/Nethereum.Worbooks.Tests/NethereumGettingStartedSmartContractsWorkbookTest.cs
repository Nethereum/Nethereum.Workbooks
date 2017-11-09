using Microsoft.CodeAnalysis.CSharp.Scripting;
using Xunit;

namespace Nethereum.Worbooks.Tests
{
    public class NethereumGettingStartedSmartContractsWorkbookTest : WorbookTest
    {
        public NethereumGettingStartedSmartContractsWorkbookTest() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = "nethereum-gettingstard-smartcontrats.workbook";

        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            //When
            var state = await CSharpScript.RunAsync(code);
            state = await state.ContinueWithAsync("return (balanceSecondAmountSend, originalBalanceFirstAmoundSend);");
            var returnValue = (dynamic) state.ReturnValue;
            //Then
            Assert.Equal(2000, returnValue.Item1);
            Assert.Equal(1000, returnValue.Item2);
        }
    }
}