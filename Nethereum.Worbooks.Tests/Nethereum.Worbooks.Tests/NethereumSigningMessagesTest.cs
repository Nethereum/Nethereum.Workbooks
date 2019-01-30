using Microsoft.CodeAnalysis.CSharp.Scripting;
using Nethereum.XUnitEthereumClients;
using Xunit;

namespace Nethereum.Worbooks.Tests
{
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_1)]
    public class NethereumSigningMessagesTest : WorbookTest
    {
        public NethereumSigningMessagesTest() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = "nethereum-signing-messages.workbook";

        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            //When
            var state = await CSharpScript.RunAsync(code);
            state = await state.ContinueWithAsync("return addressRec;");
            dynamic returnValue = (dynamic)state.ReturnValue;
            //Then
            Assert.NotNull(returnValue);
            Assert.Matches("^0x[0-9a-fA-F]{40}$", returnValue);
        }
    }
}