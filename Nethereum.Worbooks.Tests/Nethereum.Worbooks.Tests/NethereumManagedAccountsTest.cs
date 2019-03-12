using Microsoft.CodeAnalysis.CSharp.Scripting;
using System;
using System.Text;
using Nethereum.XUnitEthereumClients;
using Xunit;

namespace Nethereum.Worbooks.Tests
{
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_12)]
    public class NethereumManagedAccountsTest : WorbookTest
    {
        public NethereumManagedAccountsTest() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = ".\\nethereum-managed-accounts.workbook\\index.workbook";
        private const string  PREFIXCODESECTION = ".\\nethereum-managed-accounts.workbook\\smartContractData.csx";
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
            state = await state.ContinueWithAsync("return (contractAddress1, transactionHash);");
            var returnValue = (dynamic)state.ReturnValue;
            Assert.Matches("^0x[0-9a-fA-F]{40}$", returnValue.Item1);
            Assert.Matches("^0x[0-9a-fA-F]{64}$", returnValue.Item2);
        }
    }
}
