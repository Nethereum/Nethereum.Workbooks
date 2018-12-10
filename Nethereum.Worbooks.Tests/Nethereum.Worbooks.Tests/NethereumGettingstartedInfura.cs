using Microsoft.CodeAnalysis.CSharp.Scripting;
using Nethereum.Hex.HexConvertors.Extensions;
using Xunit;

namespace Nethereum.Worbooks.Tests
{
    public class NethereumGettingStartedInfura : WorbookTest
    {
        public NethereumGettingStartedInfura() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = "nethereum-gettingstarted-infura.workbook";

        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            //When
            var state = await CSharpScript.RunAsync(code);
            state = await state.ContinueWithAsync("return etherAmount;");
            Assert.NotNull(state.ReturnValue);
            //Assert.Equal(40, state.ReturnValue.ToString().RemoveHexPrefix().Length);
        }
    }
}