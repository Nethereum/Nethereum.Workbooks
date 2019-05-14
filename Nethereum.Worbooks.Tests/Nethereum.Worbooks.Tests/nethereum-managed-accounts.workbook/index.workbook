---
packages:
- id: Nethereum.Web3
  version: 3.0.0
uti: com.xamarin.workbook
id: d89bf107-43a9-4333-b74a-848da59c6b70
title: nethereum-managed-accounts
platforms:
- Console
---

# Using Nethereum managed accounts

This document is a Workbook, find more about workbooks' installation requirements  [here](https://docs.microsoft.com/en-us/xamarin/tools/workbooks/install).

Documentation about Nethereum can be found at: <https://docs.nethereum.com>

First, let's download the test chain matching your environment from <https://github.com/Nethereum/Testchains>

Start a Geth chain (geth-clique-linux\\, geth-clique-windows\\ or geth-clique-mac\\) using **startgeth.bat** (Windows) or **startgeth.sh** (Mac/Linux). The chain is setup with the Proof of Authority consensus and will start the mining process immediately.

**Note:** the following code relies on messages (in the context of this article, they are embedded in [smartContractData](./smartContractData.csx)). If you are not familiar with the use of messages in Nethereum, they are explained in ["Getting started with Smart Contracts"](Nethereum.Workbooks/docs/nethereum-smartcontrats-gettingstarted.workbook)

```csharp
#load "smartContractData.csx"
#r "Nethereum.Web3"
#r "Nethereum.Accounts"
```

```csharp
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Web3.Accounts.Managed;
using Nethereum.Hex.HexConvertors;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts.CQS;
using Nethereum.Util;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Contracts;
using Nethereum.Contracts.Extensions;
using System.Numerics;
```

## The use for Managed accounts

Clients retrieve the private key for an account (if stored on their keystore folder) using a password provided to decrypt the file. This is done when unlocking an account, or just at the time of sending a transaction if using `personal_sendTransaction` with a password.

Having an account unlocked for a certain period of time might be a security issue, so the prefered option in this scenario, is to use the rpc method `personal_sendTransaction`.

Nethereum.Web3 wraps this functionality by using a ManagedAccount, having the managed account storing the account address and the password information.

An instance ManagedAccount object can simply be declared using the "sender" account public address as well as its password:

```csharp
var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
var password = "password";
var account = new ManagedAccount(senderAddress, password);
var web3 = new Nethereum.Web3.Web3(account);
```

When used in conjuction with Web3, now in the same way as an "Account", you can:

### Deploy a contract

In the context of this post, the contract to deploy is `StandardTokenDeployment` (you can find it [smartContractData](./smartContractData.csx)

```csharp
var deploymentMessage = new StandardTokenDeployment
{
    TotalSupply = 100000
};
```

```csharp
var deploymentHandler = web3.Eth.GetContractDeploymentHandler<StandardTokenDeployment>();
var transactionReceipt1 = await deploymentHandler.SendRequestAndWaitForReceiptAsync(deploymentMessage);
var contractAddress1 = transactionReceipt1.ContractAddress;
```

### Interact with a Contract

Once we have deployed the contract, we can start interacting with it.

#### Querying the balance of an account

To retrieve the balance of an address we can create an instance of the BalanceFunction message and set the parameter as our account "Address", since we are the "owner" of the Token, the full balance has been assigned to us.

```csharp
var balanceOfFunctionMessage = new BalanceOfFunction()
{
    Owner = account.Address,
};

var balanceHandler = web3.Eth.GetContractQueryHandler<BalanceOfFunction>();
var balance = await balanceHandler.QueryAsync<BigInteger>(contractAddress1, balanceOfFunctionMessage);
```

#### Transfer

Lastly let's perform a token transfer. In this scenario we will need to create a TransactionHandler using the TransferFunction definition.

In the transfer message, we will include the receiver address "To", and the "TokenAmount" to transfer.

The final step is to Send the request, wait for the receipt to be “mined” and get the contract address:

```csharp
var receiverAddress = "0xde0B295669a9FD93d5F28D9Ec85E40f4cb697BAe";
var transferHandler = web3.Eth.GetContractTransactionHandler<TransferFunction>();
var transfer = new TransferFunction()
{
    To = receiverAddress,
    TokenAmount = 100
};
var transactionReceipt2 = await transferHandler.SendRequestAndWaitForReceiptAsync(contractAddress1, transfer);
var transactionHash = transactionReceipt2.TransactionHash;
```
