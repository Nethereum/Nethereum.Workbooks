---
uti: com.xamarin.workbook
id: 0230a546-0dba-4f2c-8cc9-d26049912d20
title: nethereum-estimating-gas
platforms:
- Console
packages:
- id: Nethereum.Web3
  version: 3.0.0
---

This document is a Workbook, find more about workbooks' installation requirements  [here](https://docs.microsoft.com/en-us/xamarin/tools/workbooks/install).

# Estimating the cost of a transaction with Nethereum

Documentation about Nethereum can be found at: <https://nethereum.readthedocs.io>

The purpose of this sample is to estimate the gas cost of a simple transaction and modify the assigned values of `gas` and `gasprice`.

## Ethereum and Gas: a primer

Gas is the pricing system used for running a transaction or contract in Ethereum.
The gas system is not very different from the use of kW-h for measuring electricity home use. One difference from actual energy market is that the originator of the transaction sets the price of gas, which the miner can accept or not, this causes an emergence of a market around gas. You can see the evolution of the price of gas at: <https://etherscan.io/chart/gasprice>.

The gas price per transaction or per contract is set up to deal with the Turing Complete nature of Ethereum and its EVM (Ethereum Virtual Machine Code) – the idea is to prevent infinite loops. If there is not enough Ether in the account to perform the transaction or message then it is considered invalid. The idea is to stop denial of service attacks from infinite loops, encourage efficiency in the code – and to make an attacker pay for the resources they use, from bandwidth through to CPU calculations through to storage.

Here are the terms needed to define the **gas** cost of a transaction:

* **Gas limit** refers to the maximum amount of gas you’re willing to spend on a particular transaction.

* **Gas price** refers to the amount of Ether you’re willing to pay for every unit of gas, and is usually measured in “Gwei”.

It would be difficult to send transaction without an idea of their cost in gas, fortunately  Ethereum provides ways to obtain a gas estimate prior to sending a transaction.

The following article explains how to anticipate the cost of an unsent transaction by returning an estimation.

##### A word of caution

Because of the Turing completeness of the EVM, it is easy to write functions that will take different code paths with wildly different gas costs. For example, a function could choose to take different code paths according to the value of some global state variable. The real code path taken in the function is not known until transaction execution time. Therefore the gas estimate can only give an approximation of the actual cost of a transaction.

## Quick environment setup

First, let's download the test chain matching your environment from <https://github.com/Nethereum/Testchains>

Start a Geth chain (geth-clique-linux\\, geth-clique-windows\\ or geth-clique-mac\\) using **startgeth.bat** (Windows) or **startgeth.sh** (Mac/Linux). The chain is setup with the Proof of Authority consensus and will start the mining process immediately.

**Note:** the following code relies on messages (in the context of this article, they are embedded in [smartContractData](./smartContractData.csx)). If you are not familiar with messages in Nethereum, they are used in ["Getting started with Smart Contracts"](Nethereum.Workbooks/docs/nethereum-gettingstarted-smartcontrats.workbook)

```csharp
#r "Nethereum.Web3"
```

```csharp
#r "Nethereum.Accounts"
```

```csharp
#r "Nethereum.Util"
```

```csharp
using Nethereum.Web3; using Nethereum.Web3.Accounts; using Nethereum.Util;
```

```csharp
#load "smartContractData.csx"
```

### Setting up sender address

Let's declare our private key and address as variables (we'll use the address to send the transaction) and use them to create a new account:

```csharp
var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
var account = new Nethereum.Web3.Accounts.Account(privateKey);
```

### Web3

Web3 provides a simple interaction wrapper with Ethereum clients. To create an instance of Web3, we need to supply our Account and the RPC uri of the Ethereum client. In this scenario we will only use the Account, as we will be interacting with our private test chain on the default RPC uri “http://localhost:8545”

```csharp
var web3 = new Web3(account);
```

## Transfering token

Making a transfer will change the state of the blockchain, so in this scenario we will need to create a TransactionHandler using the TransferFunction definition.

In the transfer message, we will include the receiver address `To`, and the `TokenAmount` to transfer.

The final step is to Send the request, wait for the receipt to be “mined” and included in the blockchain.

```csharp
var receiverAddress = "0xde0B295669a9FD93d5F28D9Ec85E40f4cb697BAe";
var transferHandler = web3.Eth.GetContractTransactionHandler<TransferFunction>();
var transfer = new TransferFunction()
{
    To = receiverAddress,
    TokenAmount = 100
};
var transactionReceipt = await transferHandler.SendRequestAndWaitForReceiptAsync(contractAddress, transfer);
```

### Gas Price

Nethereum automatically sets the GasPrice if not provided by using the clients "GasPrice" call, which provides the average gas price from previous blocks.

If you want to have more control over the GasPrice these can be set in both `FunctionMessages` and `DeploymentMessages`.

The GasPrice is set in "Wei" which is the lowest unit in Ethereum, so if we are used to the usual "Gwei" units, this will need to be converted using the Nethereum Conversion utilities.

```csharp
  transfer.GasPrice =  Nethereum.Web3.Web3.Convert.ToWei(25, UnitConversion.EthUnit.Gwei);
```

### Estimating Gas

Nethereum does an automatic estimation of the total gas necessary to make the function transaction by calling the `EthEstimateGas` internally with the "CallInput".

If needed, this can be done manually, using the TransactionHandler and the "transfer" transaction FunctionMessage.

```csharp
 var estimate = await transferHandler.EstimateGasAsync(contractAddress, transfer);
 transfer.Gas = estimate.Value;
```

Now the transaction will have the correct amount of `gas` at the right `gasprice`:

```csharp
var secondTransactionReceipt = await transferHandler.SendRequestAndWaitForReceiptAsync(contractAddress, transfer);
var receiptHash = secondTransactionReceipt.TransactionHash;
```
