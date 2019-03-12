using Microsoft.CodeAnalysis.CSharp.Scripting;
using Xunit;
using Nethereum.XUnitEthereumClients;
using System.Collections.Generic;

namespace Nethereum.Worbooks.Tests
{
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_6)]
    public class NethereumGettingStartedSmartContractsTest : WorbookTest
    {
        public NethereumGettingStartedSmartContractsTest() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = "nethereum-gettingstarted-smartcontracts.workbook";

        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            //When
            var state = await CSharpScript.RunAsync(code);
            state = await state.ContinueWithAsync("return (balance,signedTransaction2);");
            var returnValue = (dynamic)state.ReturnValue;
            //Then
            Assert.NotNull(returnValue.Item1);
            Assert.Matches("[0-9a-fA-F]*$", returnValue.Item2);
        }
    }
}
