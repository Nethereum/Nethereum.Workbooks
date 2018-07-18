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
            var state = await CSharpScript.RunAsync(code);
            state = await state.ContinueWithAsync("return (balanceSecondAmountSend, originalBalanceFirstAmoundSend);");
            var returnValue = (dynamic)state.ReturnValue;
           var returnValue1 = returnValue.Item1.Value;
           var returnValue2 = returnValue.Item2.Value;
            Assert.NotNull(returnValue1);
            
        }
    }
}