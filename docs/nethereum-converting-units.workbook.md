---
uti: com.xamarin.workbook
id: d143510d-16fd-4d8a-9035-8188ff4401be
title: nethereum-converting-units
platforms:
- Console
packages:
- id: Nethereum.Web3
  version: 3.0.0
---

# Converting crypto currency units with Nethereum

This document is a Workbook, find more about workbooks' installation requirements  [here](https://docs.microsoft.com/en-us/xamarin/tools/workbooks/install).

Documentation about Nethereum can be found at: <https://nethereum.readthedocs.io>

This workbook will walk you through Nethereum methods for converting Ethereum currency units.

## A primer on crypto currency units

Crypto currencies offer several units of value in order to manipulate big and small amounts accurately, exactly like one can tally an amount in cents or dollars depending on the amount to transact.

Below is the breakdown of units for **Ether**:

![](https://github.com/Nethereum/Nethereum.Workbooks/raw/master/docs/screenshots/EtherUnitConversionTable.jpg)

Now that we've covered the basics of crypto currency units in Ethereum, let’s prepare our environment to interact with a chain and learn with live code:

## Quick playground setup

First, let's download the test chain matching your environment from <https://github.com/Nethereum/Testchains>

Start a Geth chain (geth-clique-linux/, geth-clique-windows/ or geth-clique-mac/) using **_startgeth.bat_** (Windows) or **_startgeth.sh_** (Mac/Linux). The chain is setup with the Proof of Authority consensus and will start the mining process immediately.

```csharp
#r "Nethereum.Web3"
```

```csharp
#r "Nethereum.Accounts"
```

```csharp
using Nethereum.Web3;
```

```csharp
using Nethereum.Web3.Accounts;
```

After that, we need to create a web3 instance based on this new account:

```csharp
var web3 = new Web3();
```

```csharp
var account = await web3.Personal.NewAccount.SendRequestAsync("password");
```

Now that our environment is set, let's start creating transactions in various units:

## Handling units

First, let’s check the balance of one of the accounts provisioned in our chain, to do that, we can execute the GetBalance request asynchronously:

```csharp
var balance = await web3.Eth.GetBalance.SendRequestAsync("0x12890d2cce102216644c59daE5baed380d84830c");
```

By default, the returned value is in Wei (the lowest unit of value), not necessarily easy to read unless you’re really talented at Maths:

```csharp
var balanceInWei = balance.Value;
```

To make it more human friendly, we can convert the balance to Ether using the conversion utility’s "**FromWei**" method:

```csharp
var balanceInEther = Web3.Convert.FromWei(balance.Value);
```

We can even “counter convert” the balance back to wei using the “**ToWei**” method (this has no other purpose than demonstrating the method, of course):

```csharp
var BackToWei = Web3.Convert.ToWei(balanceInEther);
```