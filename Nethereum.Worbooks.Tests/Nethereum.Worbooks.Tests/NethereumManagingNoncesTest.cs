using Microsoft.CodeAnalysis.CSharp.Scripting;
using Nethereum.XUnitEthereumClients;
using Xunit;

namespace Nethereum.Worbooks.Tests
{
<<<<<<< HEAD
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_8)]
=======
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_DEFAULT)]
>>>>>>> master
    public class NethereumManagingNoncesTest : WorbookTest
    {
        public NethereumManagingNoncesTest() : base(WORKBOOK_PATH)
        {
        }

        private const string WORKBOOK_PATH = "nethereum-managing-nonces.workbook";

        [Fact]
        public async void Test()
        {
            var code = GetCodeSectionsFromWorkbook();
            //When
            var state = await CSharpScript.RunAsync(code);
<<<<<<< HEAD
            state = await state.ContinueWithAsync("return (transaction, txId);");
            dynamic returnValue = (dynamic)state.ReturnValue;
            //Then
            Assert.NotNull(returnValue);
            Assert.Matches("^0x[0-9a-fA-F]{64}$", returnValue.Item1);
            Assert.Matches("^0x[0-9a-fA-F]{64}$", returnValue.Item2);
        }
    }
}
=======
            state = await state.ContinueWithAsync("return transaction;");
            dynamic returnValue = (dynamic)state.ReturnValue;
            //Then
            Assert.NotNull(returnValue);
            Assert.Matches("^0x[0-9a-fA-F]{64}$", returnValue);
        }
    }
}
>>>>>>> master
