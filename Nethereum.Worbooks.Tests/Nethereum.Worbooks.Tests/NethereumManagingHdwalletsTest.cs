using Microsoft.CodeAnalysis.CSharp.Scripting;
using System;
using System.Text;
using Nethereum.XUnitEthereumClients;
using Xunit;

namespace Nethereum.Worbooks.Tests
{
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_17)]
    public class NethereumManagingHdwalletsTest : WorbookTest
    {
        public NethereumManagingHdwalletsTest() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = ".\\nethereum-managing-hdwallets.workbook";
        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            var state = await CSharpScript.RunAsync(code);
            state = await state.ContinueWithAsync("return (account1.Address, recoveredAccount.Address);");
            var returnValue = (dynamic)state.ReturnValue;
            Assert.Matches("^0x[0-9a-fA-F]{40}$", returnValue.Item1);
            Assert.Matches("^0x[0-9a-fA-F]{40}$", returnValue.Item2);
        }
    }
}
