using Microsoft.CodeAnalysis.CSharp.Scripting;
using Xunit;
using Nethereum.XUnitEthereumClients;
using System.Collections.Generic;

namespace Nethereum.Worbooks.Tests
{
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_14)]
    public class NethereumGettingStartedSmartContratsUntypedTest : WorbookTest
    {
        public NethereumGettingStartedSmartContratsUntypedTest() : base(WORKBOOK_PATH)
        {
        }


        private const string WORKBOOK_PATH = "nethereum-gettingstarted-smartcontracts-untyped.workbook";

        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            //When
            var state = await CSharpScript.RunAsync(code);
            state = await state.ContinueWithAsync("return (transactionHashSecondAmountSend, originalBalanceFirstAmoundSend);");
            var returnValue = (dynamic)state.ReturnValue;
            //Then
            Assert.Matches("^0x[0-9a-fA-F]{64}$", returnValue.Item1);
            Assert.NotNull(returnValue.Item2);
        }
    }
}
