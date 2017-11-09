using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Xunit;

namespace Nethereum.Worbooks.Tests
{
    public class NethereumGettingStartedSmartContractsWorkbookTest
    {
        private readonly MardownHelper _mardownHelper = new MardownHelper();

        [Fact]
        public async void Test()
        {
            var workbookText = System.IO.File.OpenText(workbook).ReadToEnd();
            var code = _mardownHelper.ExtractCodeFromMarkdown(workbookText);

            var state = await CSharpScript.RunAsync(code);
            state = await state.ContinueWithAsync("return (balanceSecondAmountSend, originalBalanceFirstAmoundSend);");
            var returnValue = (dynamic)state.ReturnValue;
            Assert.Equal(2000, returnValue.Item1);
            Assert.Equal(1000, returnValue.Item2);
        }

        private string workbook = @"nethereum-gettingstard-smartcontrats.workbook";
       
    }
}
