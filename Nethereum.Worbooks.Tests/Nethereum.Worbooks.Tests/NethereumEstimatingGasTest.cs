using Microsoft.CodeAnalysis.CSharp.Scripting;
using Nethereum.XUnitEthereumClients;
using Xunit;

namespace Nethereum.Worbooks.Tests
{
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_15)]
    public class NethereumEstimatingGasTest : WorbookTest
    {
        public NethereumEstimatingGasTest() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = ".\\nethereum-estimating-gas.workbook\\index.workbook";
        private const string  PREFIXCODESECTION = ".\\nethereum-estimating-gas.workbook\\smartContractData.csx";
        [Fact]
        public async void Test()
        {
            var prefixCode = LoadCodeSection(PREFIXCODESECTION);
            var code = GetCodeSectionsFromWorkbook();
            var usingsCode = ExtractUsingStatements(code);
            var Rs = ExtractRStatements(code);
            var usingsPrefix = ExtractUsingStatements(prefixCode);
            prefixCode = RemoveLoadSections(prefixCode); 
            code = RemoveLoadSections(code);
            var state = await CSharpScript.RunAsync(Rs + usingsCode+usingsPrefix +prefixCode+code);
            state = await state.ContinueWithAsync("return receiptHash;");
            dynamic returnValue = (dynamic)state.ReturnValue;
            //Then
            Assert.Matches("^0x[0-9a-fA-F]*$", returnValue);
        }
    }
}