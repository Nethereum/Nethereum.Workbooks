---
uti: com.xamarin.workbook
id: 701d2cce-38a5-43b6-a682-e7684c44d480
title: nethereum-gettingstarted-infura
platforms:
- Console
packages:
- id: Nethereum.Web3
  version: 3.0.0
---

# Getting started using Infura with Nethereum

This document is a Workbook, find more about workbooks' installation requirements  [here](https://docs.microsoft.com/en-us/xamarin/tools/workbooks/install).

Documentation about Nethereum can be found at: <https://docs.nethereum.com>

This sample will take you through the steps of connecting to [Infura](https://www.infura.io) retrieve the balance of an account from the mainnet (live Ethereum) as well as check on the chain ID and latest block number.


INFURA runs ethereum nodes and provides access to them via api to eliminate the need to run and update your own infrastructure.

The first step to use INFURA is to [sign up](https://infura.io/register) and get an API key. The next step is to choose which Ethereum network to connect to — either the mainnet, or the Kovan, Goerli, Rinkeby, or Ropsten test networks. Both of these will be used in the url we use to initial Nethereum with the format:`https://<network>.infura.io/v3/YOUR-PROJECT-ID`.

For this sample, we’ll use a special API key `7238211010344719ad14a89db874158c`, but for your own project you’ll need your own key.

```csharp
#r "Nethereum.Web3"
```

```csharp
using Nethereum.Web3;
```

Let’s create an instance of Web3, with the infura url for mainnet.

```csharp
var web3 = new Web3("https://mainnet.infura.io/v3/7238211010344719ad14a89db874158c");
```

Using the Eth API we can execute the GetBalance request asynchronously, for our selected account. In this scenario the chosen account belongs to the Ethereum Foundation. “0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae”

```csharp
var balance = await web3.Eth.GetBalance.SendRequestAsync("0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae");
```

The amount returned is in Wei, the lowest unit of value. We can convert this to Ether using the Convertion utility:

```csharp
var etherAmount = Web3.Convert.FromWei(balance.Value);
```

Using the Net API, we can call and find out which network we’re connected to. This will change depending on which network we chose to connect to previously. For example, Kovan will return `42` and mainnet would be `1`.

```csharp
var networkId = await web3.Net.Version.SendRequestAsync();
```

Next, using the Eth API, we’ll call to get the latest block which has been mined in this network.

```csharp
var latestBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
var latestBlock = await web3.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(latestBlockNumber);
```

That’s a quick demo of using Nethereum with INFURA. One important thing to know when using hosted infrastructure like Infura is it doesn’t store any private keys, so any signing must be done locally and then the raw transaction passed on to the service. Nethereum makes this easy with the `Account` object. See the [Using account objects](https://docs.nethereum.com/en/latest/Nethereum.Workbooks/docs/nethereum-using-account-objects/#sending-a-transaction) for more details.

Note: some communication errors can occur with INFURA if INFURA's API and your app can't agree on what version of TLS to use. .Net 4.5 and earlier will default to TLS v1, with TLS v1.2 deactivated if it's included in the framework. (In .Net 4.6.*, v1.2 is the default.)
To enable v1.2 in 4.5.2 you can use:
`System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;` Notice the use of |= to turn on 1.2 without affecting other protocols (that way you remain able to take advantage of future TLS versions that may become the default values in future updates to .NET).
