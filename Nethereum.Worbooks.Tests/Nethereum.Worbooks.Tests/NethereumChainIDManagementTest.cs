using Microsoft.CodeAnalysis.CSharp.Scripting;
using Nethereum.Hex.HexConvertors.Extensions;
using Xunit;
using Nethereum.XUnitEthereumClients;

namespace Nethereum.Worbooks.Tests
{

    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_DEFAULT)]
    public class NethereumChainIDManagementTest : WorbookTest
    {
        public NethereumChainIDManagementTest() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = "Nethereum-ChainID-Management.workbook";

        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            //When
            var state = await CSharpScript.RunAsync(code);
            state = await state.ContinueWithAsync("return balance;");
            var returnValue = (dynamic)state.ReturnValue;
            Assert.NotNull(returnValue);
        }
    }
}