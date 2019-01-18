using Microsoft.CodeAnalysis.CSharp.Scripting;
using Nethereum.XUnitEthereumClients;
using Xunit;

namespace Nethereum.Worbooks.Tests
{
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_DEFAULT)]
    public class NethereumEventsGettingStartedTest : WorbookTest
    {
        public NethereumEventsGettingStartedTest() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = "nethereum-events-gettingstarted.workbook";

        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            //When
            var state = await CSharpScript.RunAsync(code);
            state = await state.ContinueWithAsync("return contractAddress2;");
            dynamic returnValue = (dynamic)state.ReturnValue;
            //returnValue = returnValue.Item1.Value;
            //Then
            Assert.NotNull(returnValue);
            Assert.Matches("^0x[0-9a-fA-F]{40}$", returnValue);
        }
    }
}