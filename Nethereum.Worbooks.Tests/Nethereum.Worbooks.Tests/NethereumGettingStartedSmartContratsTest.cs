using Microsoft.CodeAnalysis.CSharp.Scripting;
using Xunit;
using Nethereum.XUnitEthereumClients;
using System.Collections.Generic;

namespace Nethereum.Worbooks.Tests
{
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_6)]
    public class NethereumGettingStartedSmartContratsTest : WorbookTest
    {
        public NethereumGettingStartedSmartContratsTest() : base(WORKBOOK_PATH)
        {
        }


        private const string WORKBOOK_PATH = "nethereum-smartcontrats-gettingstarted.workbook";

        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            //When
            var state = await CSharpScript.RunAsync(code);
             state = await state.ContinueWithAsync("return (transfer.Nonce, balance);");
            var returnValue = (dynamic)state.ReturnValue;
            //Then
            Assert.Equal(2, returnValue.Item1);
            Assert.Equal(100000, returnValue.Item2);
        }
    }
}