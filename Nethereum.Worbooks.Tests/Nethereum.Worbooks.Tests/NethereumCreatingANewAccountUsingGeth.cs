using Microsoft.CodeAnalysis.CSharp.Scripting;
using Nethereum.Hex.HexConvertors.Extensions;
using Xunit;

namespace Nethereum.Worbooks.Tests
{
    public class NethereumCreatingANewAccountUsingGeth:WorbookTest
    {
        private const string WORKBOOK_PATH = "nethereum-creating-a-new-account-using-geth.workbook";
        public NethereumCreatingANewAccountUsingGeth() : base(WORKBOOK_PATH)
        {
        }

        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            //When
            var state = await CSharpScript.RunAsync(code);
            state = await state.ContinueWithAsync("return account;");
            Assert.NotNull(state.ReturnValue);
            Assert.Equal(40, state.ReturnValue.ToString().RemoveHexPrefix().Length);
        }
    }
}