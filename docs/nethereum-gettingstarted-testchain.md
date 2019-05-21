# TestChain Getting Started Guide

This document is a Workbook, an interactive document where you can run code.
To run workbooks natively, you can:

* [Install the runtime](https://docs.microsoft.com/en-us/xamarin/tools/workbooks/install)

* [Download the native file for this document](http://docs.nethereum.com/en/latest/Nethereum.Workbooks/docs/nethereum-gettingstarted-testchain.workbook)

The entirety of Nethereum workbooks can be found [here](https://github.com/Nethereum/Nethereum.Workbooks)

Documentation about Nethereum can be found at: <https://docs.nethereum.com>

Blockchain developing often requires to run a local Blockchain client (AKA: TestChain). This is to make sure your work remains private and that any sent transaction gets a fast response.

In order to speed up the process, this repo contains all you need to spin up a local TestChain in a few minutes. Each of these chains uses PoA (Proof of Authority) as a consensus model for faster response. They all launch using provided scripts, automatically provided with accounts and passwords.

First, let's download the test chain matching your environment from <https://github.com/Nethereum/Testchains>

Start a Geth chain (geth-clique-linux\\, geth-clique-windows\\ or geth-clique-mac\\) using **startgeth.bat** (Windows) or **startgeth.sh** (Mac/Linux). The chain is setup with the Proof of Authority consensus and will start the mining process immediately.

```csharp
#r "Nethereum.Web3"
```

First, we need to new up a web3 object for localhost:8545 (this uses RPC)

```csharp
var web3 = new Nethereum.Web3.Web3();
```

Are we mining? (startgeth.sh|bat auto-starts mining)

```csharp
var isMining = await web3.Eth.Mining.IsMining.SendRequestAsync();
```

List all pre-defined accounts in the devchain

```csharp
var accounts = await web3.Eth.Accounts.SendRequestAsync();
```

Get the balance for the default account

```csharp
var balance = await web3.Eth.GetBalance.SendRequestAsync("0x12890d2cce102216644c59dae5baed380d84830c");
```
