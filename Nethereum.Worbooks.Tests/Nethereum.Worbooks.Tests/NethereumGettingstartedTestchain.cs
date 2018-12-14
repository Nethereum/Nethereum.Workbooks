using Microsoft.CodeAnalysis.CSharp.Scripting;
using Nethereum.Hex.HexConvertors.Extensions;
using Xunit;

namespace Nethereum.Worbooks.Tests
{
    public class NethereumGettingstartedTestchain : WorbookTest
    {
        public NethereumGettingstartedTestchain() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = "nethereum-gettingstarted-testchain.workbook";

        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            //When
            var state = await CSharpScript.RunAsync(code);
            state = await state.ContinueWithAsync("return (isMining, balance);");
            var returnValue = (dynamic)state.ReturnValue;
            Assert.True(returnValue.Item1);
            Assert.NotNull(returnValue.Item2);
        }
    }
}