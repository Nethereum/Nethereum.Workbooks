using Microsoft.CodeAnalysis.CSharp.Scripting;
using Xunit;

namespace Nethereum.Worbooks.Tests
{
    public class NethereumSendingTransactions : WorbookTest
    {
        public NethereumSendingTransactions() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = "nethereum-sending-transactions.workbook";

        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            //When
            var state = await CSharpScript.RunAsync(code);
             state = await state.ContinueWithAsync("return (transaction, transactionManagedAccount);");
            var returnValue = (dynamic)state.ReturnValue;
            Assert.Matches("^0x[0-9a-fA-F]{64}$", returnValue.Item1);
            Assert.Matches("^0x[0-9a-fA-F]{64}$", returnValue.Item2);
        }
    }
}