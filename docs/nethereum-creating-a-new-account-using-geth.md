---
uti: com.xamarin.workbook
id: 5ae81721-2480-4fb0-baa6-62e1f79aa238
title: nethereum-creating-a-new-account-using-geth
platforms:
- Console
packages:
- id: Nethereum.Web3
  version: 3.0.0
---

## Creating a new Account using Geth Personal Api

This document is a Workbook, find more about workbooks' installation requirements  [here](https://docs.microsoft.com/en-us/xamarin/tools/workbooks/install).

Documentation about Nethereum can be found at: <https://docs.nethereum.com>

## Prerequisites:

First, let's download the test chain matching your environment from <https://github.com/Nethereum/Testchains>

Start a Geth chain (geth-clique-linux\\, geth-clique-windows\\ or geth-clique-mac\\) using **startgeth.bat** (Windows) or **startgeth.sh** (Mac/Linux). The chain is setup with the Proof of Authority consensus and will start the mining process immediately.

```csharp
#r "Nethereum.Web3"
```

```csharp
using Nethereum.Web3;
```

Now, let's create a new instance of Web3:

```csharp
var web3 = new Web3();
```

Using the Personal API we can now execute the request "NewAccount" with a password to encrypt the account storage file:

```csharp
var account = await web3.Personal.NewAccount.SendRequestAsync("password");
```
