---
uti: com.xamarin.workbook
id: 7c6a7c28-9e2c-4536-8cb3-841b6173cacb
title: nethereum-managing-nonces
platforms:
- Console
packages:
- id: Nethereum.Web3
  version: 2.0.1
---
# Managing nonces with Nethereum
To prevent replay attacks (submitting the same transaction several times) Ethereum provides a transaction counter: the `nonce` parameter. Nonce keeps track of the number of times a transaction has been run by an account. 

When making a transaction in Ethereum, a consecutive number should be attached to each transaction on the same account. Each node will process transactions from a specific account in a strict order according to the value of its nonce.

Therefore, failing to increment this value correctly can result in different kinds of errors. For instance, let’s say the latest transaction nonce was 121:

Reusing nonce: if we send a new transaction for the same account with a nonce of either 121 or below, the node will reject it.
“Gaps”: if we send a new transaction with a nonce of either 123 or higher, the transaction will not be processed until this gap is closed, i.e. until a transaction with nonce 122 has been processed.

This workbook shows how Nethereum helps manage `nonce`.


Let's first set our environment:

## prerequisites:

first, let's download the test chain matching your environment from <https://github.com/nethereum/testchains>

start a geth chain (geth-clique-linux\\, geth-clique-windows\\ or geth-clique-mac\\) using **startgeth.bat** (windows) or **startgeth.sh** (mac/linux). the chain is setup with the proof of authority consensus and will start the mining process immediately.

we then need to add nethereum's nuget package:
```csharp
#r "nethereum.web3"
```
after that, we will need to add `using` statements:
```csharp
using nethereum.web3;
using nethereum.web3.accounts;
using nethereum.web3.accounts.managed;
using nethereum.signer;
using nethereum.keystore;
```

## Nonce management with Nethereum `accounts` objects

In most cases, Nethereum takes care of incrementing the `nonce` automatically (unless you need to sign a raw transaction manually, we'll explain that in the next chapter).

Once you have loaded your private keys into your account, if Web3 is instantiated with that account all the transactions made using the TransactionManager, Contract deployment or Functions will be signed offline using the latest nonce.

Nonce management is insured whether you are using an `account` or a `managed account` object.

Example:
This example shows what happens to the `nonce` value when we send a transaction with a Nethereum account.
We first need create an instance of a managed account, then use it to 
 instantiate a `web3` object.

```csharp
//`ecKey` generates a private key
var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
//`privateKey` evaluates to the generated private key
`var privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();
// `account` evaluates to a Nethereum `account` object
var account = new Nethereum.Accounts.Account(privateKey);
// `web3` is the Web3 instance using the new `account` as constructor
var web3 = new Web3(account); 
```
Let's now examine what happens to the `nonce` value before and after we send a transaction:

#### Before a transaction is sent
```csharp
var txCount = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(account.address);
```

Now, let's send a simple transaction:``
```csharp
var recipientAddress = “0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae”;
var transaction = await web3.TransactionManager.SendTransactionAsync(account.Address, recipientAddress, new HexBigInteger(20)); 
```


#### After a transaction has been sent
```csharp
var txCount = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(account.address);
```
## Nonce management signing a row transaction manually
Methods dealing with nonce management

GetTransactionCount
Nethereum does that in the background
so, if you send a transaction with an `account` object or a `managed account` object, the nonce management is done automatically
With the nonce service
in this particular case http://nethereum.readthedocs.io/en/latest/introduction/web3/#offline-transaction-signing
you need to add it yourself
correct?
Yes but that is the do it yourself scenario
The account uses the transaction manager
That uses the nonce service
