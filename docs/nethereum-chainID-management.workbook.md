---
uti: com.xamarin.workbook
id: 4e0b3afb-0940-4d51-9f9b-e4baeb5714d4
title: Nethereum-ChainID-Management
platforms:
- Console
packages:
- id: Nethereum.Web3
  version: 3.0.0
---

This document is a Workbook, find more about workbooks' installation requirements  [here](https://docs.microsoft.com/en-us/xamarin/tools/workbooks/install).

## Pre-requisites:

Download the test chain matching your environment from https://github.com/Nethereum/Testchains

Start a Geth chain ( **geth-clique-linux\_** **_geth-clique-windows_** **_geth-clique-mac_**) using startgeth.bat (Windows) or startgeth.sh (Mac/Linux). The chain is setup with the Proof of Authority consensus and will start the mining process immediately.

```csharp
#r "Nethereum.Web3"
```

```csharp
#r "Nethereum.Accounts"
```

```csharp
using Nethereum.Web3; using Nethereum.Signer; using Nethereum.Web3.Accounts; using Nethereum.Util; using Nethereum.Hex.HexConvertors.Extensions; using Nethereum.RPC.Eth.DTOs; using Nethereum.Hex.HexTypes;
```

# Chain ID management for replay attack protection

This workbook explains what a replay attack is and how Nethereum allows you to protect your code against them.

## Replay Attack

Ethereum makes it possible to send the same transaction across different chains, hence the term "replay attack". For instance, it is possible to issue a fund transfer on a testchain and then
perform the same transfer over the MainNet (with real funds). This vulnerability is due to the fact that the same accounts can exist in any Ethereum chain, protected by the same privateKey.

To counteract this issue, an Ethereum fix was implemented (the improvement name is [EIP155](https://github.com/Nethereum/Nethereum.Workbooks/issues/10)) allowing the insertion of the ChainID data in signed transactions. Thanks to this improvement it is now possible to force a transaction to only run on a specific chain by including its ID when signed.

The preconfigured chainIds can be found in Nethereum.Signer.Chain:

![](https://github.com/Nethereum/Nethereum.Workbooks/raw/master/docs/screenshots/NethereumChainIDManagement1.jpg)

To configure the chainId in geth, edit the genesis as follows (example configuration):

![](https://github.com/Nethereum/Nethereum.Workbooks/raw/master/docs/screenshots/NethereumChainIDManagement2.jpg)

To sign a transaction using the ChainID attribute, we need to create an instance of the "Account" object using our private key and ChainID as arguments.

First, we need to declare our private key:

```csharp
var privatekey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
```

Then we can create an Account instance as follows, using the chainId from the MainNet:

```csharp
var account = new Account(privatekey, Chain.MainNet);
```

or just using our custom chainId as such:

```csharp
account =  new Account(privatekey, 444444444500);
```

For this sample we will use our custom chainId already set in our testnet **444444444500**.

We now can create a new instance of Web3 using the account configured with the chainId. Internally the TransactionManager will use this chainId to sign all transactions.

```csharp
var web3 = new Web3(account);
```

Let's use it in a simple example, for example the transfer of Ether.

```csharp
var toAddress = "0x13f022d72158410433cbd66f5dd8bf6d2d129924";
```

First let's convert 1 Ether to Wei.

```csharp
var wei = Web3.Convert.ToWei(1);
```

And then use the TransactionManager to execute the transfer and wait for the receipt.

```csharp
 var transactionReceipt = await web3.TransactionManager.TransactionReceiptService.SendRequestAndWaitForReceiptAsync(
               new TransactionInput() {From = account.Address, To = toAddress, Value = new HexBigInteger(wei)}, null);
```

Finally, we can see that the receiver’s address balance has increased by 1 Ether

```csharp
 var balance = await web3.Eth.GetBalance.SendRequestAsync("0x13f022d72158410433cbd66f5dd8bf6d2d129924");
 var amountInEther = Web3.Convert.FromWei(balance.Value);
```
