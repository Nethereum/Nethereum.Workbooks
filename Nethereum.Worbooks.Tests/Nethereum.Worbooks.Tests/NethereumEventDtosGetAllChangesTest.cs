using Microsoft.CodeAnalysis.CSharp.Scripting;
using Nethereum.XUnitEthereumClients;
using Xunit;

namespace Nethereum.Worbooks.Tests
{
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_3)]
    public class NethereumEventDtosGetAllChangesWorkbookTest : WorbookTest
    {
        public NethereumEventDtosGetAllChangesWorkbookTest() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = "nethereum-eventdtos-getallchanges.workbook";

        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            //When
            var state = await CSharpScript.RunAsync(code);
            state = await state.ContinueWithAsync("return (gas, logs2);");
            dynamic returnValue = (dynamic)state.ReturnValue;
            returnValue = returnValue.Item1.Value;
            //Then
            Assert.NotNull(returnValue);
        }
    }
}