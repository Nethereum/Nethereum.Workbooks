# Using Receipt Status

This document is a Workbook, an interactive document where you can run code.
To run workbooks natively, you can:

* [Install the runtime](https://docs.microsoft.com/en-us/xamarin/tools/workbooks/install)

* [Download the native file for this document](http://docs.nethereum.com/en/latest/Nethereum.Workbooks/docs/nethereum-receipt-status.workbook/index.md)

The entirety of Nethereum workbooks can be found [here](https://github.com/Nethereum/Nethereum.Workbooks)


Since the Byzantium fork, Ethereum provides with a way to know if a transaction succeeded by checking its receipt `status`. A receipt status can have a value of `0` or `1` which translate into:

* `0` transaction has failed (for whatever reason)

* `1` transaction was succesful.

## Prerequisites:

First, let's download the test chain matching your environment from <https://github.com/nethereum/testchains>

Start a geth chain (geth-clique-linux\\, geth-clique-windows\\ or geth-clique-mac\\) using **startgeth.bat** (windows) or **startgeth.sh** (mac/linux). the chain is setup with the proof of authority consensus and will start the mining process immediately.

**Note:** the following code relies on messages (in the context of this article, they are embedded in [receiptStatusData](./receiptStatusData.csx)). If you are not familiar with the use of messages in Nethereum, ["Getting started with Smart Contracts" will help you see them work in context.](Nethereum.Workbooks/docs/nethereum-smartcontrats-gettingstarted.workbook)

## How to retrieve a transaction status with Nethereum

Ethereum allows to retrieve this property after retrieving a transaction receipt via methods such as `SendTransactionAndWaitForReceiptAsync`,  `SendRequestAndWaitForReceiptAsync`, `TransferRequestAndWaitForReceiptAsync`, or `DeployContractAndWaitForReceiptAsync`

```csharp
#r "nethereum.web3"
```

```csharp
#r "nethereum.Accounts"
```

```csharp
#load "receiptStatusData.csx"
```

```csharp
using Nethereum.RPC.Eth.Transactions;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Web3.Accounts.Managed;
using Nethereum.Signer;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.KeyStore;
using Nethereum.Hex.HexConvertors;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.NonceServices;
using Nethereum.RPC.TransactionReceipts;
using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;
```

We'll demonstrate how to use transactions `status` after deploying a contract and after a token transfer.

We first need to create an instance of an account, then use it to instantiate a `web3` object.

Let's first declare our new `Account`:

```csharp
var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
var account = new Nethereum.Web3.Accounts.Account(privateKey);
```

* `web3` is the Web3 instance using the new `account` as constructor

```csharp
var web3 = new Web3(account);
```

Let's now deploy our contract.
In the context of this post, the contract to deploy is `StandardTokenDeployment` (you can find it [receiptStatusData](./receiptStatusData.csx)).We'll deploy this contract with an initial supply of 100000.

```csharp
var deploymentMessage = new StandardTokenDeployment
{
    TotalSupply = 100000
};
```

The deployment method will return a receipt object, here `deploymentTransactionReceipt`

```csharp
var deploymentHandler = web3.Eth.GetContractDeploymentHandler<StandardTokenDeployment>();
var deploymentTransactionReceipt = await deploymentHandler.SendRequestAndWaitForReceiptAsync(deploymentMessage);
```

We can now get the value of the transaction `status` which will give us the outcome of our deployment transaction, `1` for success, `0` for failure.

```csharp
var deploymentValidityStatus = deploymentTransactionReceipt.Status.Value;
```

The same applies for any other transaction, for example, we can assess if a token transfer has been succesful:

```csharp
var receiverAddress = "0xde0B295669a9FD93d5F28D9Ec85E40f4cb697BAe";
var transferHandler = web3.Eth.GetContractTransactionHandler<TransferFunction>();
var transfer = new TransferFunction()
{
    To = receiverAddress,
    TokenAmount = 100
};
var transactionReceipt = await transferHandler.SendRequestAndWaitForReceiptAsync(deploymentTransactionReceipt.ContractAddress, transfer);
var transferValidityStatus = transactionReceipt.Status.Value;
```
