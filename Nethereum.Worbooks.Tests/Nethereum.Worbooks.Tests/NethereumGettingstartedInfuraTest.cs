using Microsoft.CodeAnalysis.CSharp.Scripting;
using Nethereum.Hex.HexConvertors.Extensions;
using Xunit;
using Nethereum.XUnitEthereumClients;

namespace Nethereum.Worbooks.Tests
{

    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_5)]
    public class NethereumGettingStartedInfuraTest : WorbookTest
    {
        public NethereumGettingStartedInfuraTest() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = "nethereum-gettingstarted-infura.workbook";

        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            code = CorrectKeystorePath(code);
            //When
            var state = await CSharpScript.RunAsync(code);
            state = await state.ContinueWithAsync("return (etherAmount, transaction);");
            dynamic returnValue = (dynamic)state.ReturnValue;
            //Then
            Assert.NotNull(returnValue.Item1);
            Assert.Matches("^0x[0-9a-fA-F]{64}$", returnValue.Item2);
        }
    }
}
