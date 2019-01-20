using Microsoft.CodeAnalysis.CSharp.Scripting;
using Nethereum.XUnitEthereumClients;
using Xunit;

namespace Nethereum.Worbooks.Tests
{
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_DEFAULT)]
    public class NethereumChainIdManagement : WorbookTest
    {
        public NethereumChainIdManagement() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = "Nethereum-ChainID-Management.workbook";

        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            //When
            var state = await CSharpScript.RunAsync(code);
            state = await state.ContinueWithAsync("return (transactionReceipt, balance);");
            dynamic returnValue = (dynamic)state.ReturnValue;
            //Then
            Assert.NotNull(returnValue.Item1);
            Assert.Matches("^0x[0-9a-fA-F]{16}$", returnValue.Item2.HexValue);
        }
    }
}