using Microsoft.CodeAnalysis.CSharp.Scripting;
using Xunit;

namespace Nethereum.Worbooks.Tests
{
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
<<<<<<< HEAD
            dynamic returnValue = (dynamic)state.ReturnValue;
            //Then
            Assert.Equal(25534444, returnValue.Item1.Value);
=======
            var returnValue = (dynamic)state.ReturnValue;
            //Then
            Assert.Equal(25535, returnValue.Item1.Value);
>>>>>>> f9fc970b817ce2fb25f4cd01648f35a84620bc40
            Assert.Equal(1, returnValue.Item2.Count);

        }
    }
}