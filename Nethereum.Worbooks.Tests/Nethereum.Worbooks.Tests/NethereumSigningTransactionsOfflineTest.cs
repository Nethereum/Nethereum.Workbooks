using Microsoft.CodeAnalysis.CSharp.Scripting;
using Nethereum.XUnitEthereumClients;
using Xunit;

namespace Nethereum.Worbooks.Tests
{
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_11)]
    public class NethereumSigningTransactionsOfflineTest : WorbookTest
    {
        public NethereumSigningTransactionsOfflineTest() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = "nethereum-signing-transactions-offline.workbook";

        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            //When
            var state = await CSharpScript.RunAsync(code);
            state = await state.ContinueWithAsync("return (encoded, txId);");
            dynamic returnValue = (dynamic)state.ReturnValue;
            Assert.NotNull(returnValue.Item1);
            Assert.Matches("^[0-9a-fA-F]{204}$", returnValue.Item1);
            Assert.NotNull(returnValue.Item2);
            Assert.Matches("^0x[0-9a-fA-F]{64}$", returnValue.Item2);
        }
    }
}