using Microsoft.CodeAnalysis.CSharp.Scripting;
using Xunit;
using Nethereum.XUnitEthereumClients;

namespace Nethereum.Worbooks.Tests
{
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_5)]
    public class NethereumGettingStartedNodesmithTest : WorbookTest
    {
        public NethereumGettingStartedNodesmithTest() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = "nethereum-gettingstarted-nodesmith.workbook";

        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            var state = await CSharpScript.RunAsync(code);
            state = await state.ContinueWithAsync("return latestBlock;");
            Assert.NotNull(state.ReturnValue);

            // Get the retuned block hash and make sure it's valid
            dynamic returnValue = (dynamic)state.ReturnValue;
            var blockHash = returnValue.BlockHash;
            Assert.Matches("^0x[0-9a-fA-F]*$", blockHash);
        }
    }
}
