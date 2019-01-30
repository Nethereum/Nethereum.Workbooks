using Microsoft.CodeAnalysis.CSharp.Scripting;
using Nethereum.Hex.HexConvertors.Extensions;
using Xunit;
using Nethereum.XUnitEthereumClients;

namespace Nethereum.Worbooks.Tests
{

    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_2)]
    public class NethereumCreatingANewAccountUsingGethTest : WorbookTest
    {
        public NethereumCreatingANewAccountUsingGethTest() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = "nethereum-creating-a-new-account-using-geth.workbook";

        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            //When
            var state = await CSharpScript.RunAsync(code);
            state = await state.ContinueWithAsync("return account;");
            var returnValue = (dynamic)state.ReturnValue;
            Assert.NotNull(returnValue);
            Assert.Matches("^0x[0-9a-fA-F]{40}$", returnValue);
        }
    }
}