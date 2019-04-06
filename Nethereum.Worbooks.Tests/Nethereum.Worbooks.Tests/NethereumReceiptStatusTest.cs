using Microsoft.CodeAnalysis.CSharp.Scripting;
using System;
using System.Text;
using Nethereum.XUnitEthereumClients;
using Xunit;
using System.Numerics;

namespace Nethereum.Worbooks.Tests
{
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_17)]
    public class NethereumReceiptStatusTest : WorbookTest
    {
        public NethereumReceiptStatusTest() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = ".\\nethereum-receipt-status.workbook\\index.workbook";
        private const string  PREFIXCODESECTION = ".\\nethereum-receipt-status.workbook\\receiptStatusData.csx";
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
            state = await state.ContinueWithAsync("return (deploymentValidityStatus, transferValidityStatus);");
            var returnValue = (dynamic)state.ReturnValue;
            Assert.IsType<BigInteger>(returnValue.Item1);
            Assert.IsType<BigInteger>(returnValue.Item2);
        }
    }
}
