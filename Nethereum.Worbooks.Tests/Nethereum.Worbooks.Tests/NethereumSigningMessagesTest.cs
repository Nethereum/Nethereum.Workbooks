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
             state = await state.ContinueWithAsync("return (addressRec1, addressRec2);");
            var returnValue = (dynamic)state.ReturnValue;
            Assert.Matches("^0x[0-9a-fA-F]{40}$", returnValue.Item1);
            Assert.Matches("^0x[0-9a-fA-F]{40}$", returnValue.Item2);
        }
    }
}