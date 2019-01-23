---
uti: com.xamarin.workbook
id: 5808030e-e736-41a5-b749-8556e0b08727
title: nethereum-eventdtos-getallchanges
platforms:
- Console
packages:
- id: Nethereum.Web3
  version: 3.0.0
---

# Setting Up Events Polling Services Using Nethereum

This document is a Workbook, find more about workbooks' installation requirements  [here](https://docs.microsoft.com/en-us/xamarin/tools/workbooks/install).

This workbook explains how to set up a polling service tracking events occurring on a Smart Contract.

Background:

In the Ethereum environment, functions don't return anything. In order to compensate for that limitation,  Solidity offers a way to log state changes which is called Event. The following explains how to track events using Nethereum.

## Prerequisites:

Download the test chain matching your environment from https://github.com/Nethereum/Testchains

Start a Geth chain ( **geth-clique-linux\_** **_geth-clique-windows_** **_geth-clique-mac_**) using startgeth.bat (Windows) or startgeth.sh (Mac/Linux). The chain is setup with the Proof of Authority consensus and will start the mining process immediately.

```csharp
#r "Nethereum.Web3"
```

```csharp
#r "Nethereum.Accounts"
```

```csharp
using Nethereum.Web3;using Nethereum.Web3.Accounts;using System.Numerics; using Nethereum.RPC.Eth.DTOs;using Nethereum.ABI.FunctionEncoding.Attributes;
```

Here is the Smart Contract we are going to log using events:

![](https://github.com/Nethereum/Nethereum.Workbooks/raw/master/docs/screenshots/testEventContract.jpg)

In order to deploy your Smart Contract, you will need first to compile it  (using the VS Code’s “Solidity” extension, for instance), and declare your ABI and ByteCode in your CSharp class from the compilation output.

```csharp
public static string ABI = @"[{'constant':false,'inputs':[],'name':'addBid','outputs':[],'payable':true,'stateMutability':'payable','type':'function'},{'inputs':[],'payable':true,'stateMutability':'payable','type':'constructor'},{'payable':true,'stateMutability':'payable','type':'fallback'},{'anonymous':false,'inputs':[{'indexed':true,'name':'sender','type':'address'},{'indexed':false,'name':'amount','type':'uint256'},{'indexed':false,'name':'encryptedRate','type':'string'},{'indexed':false,'name':'time','type':'uint256'}],'name':'BidAdded','type':'event'},{'anonymous':false,'inputs':[{'indexed':false,'name':'currentState','type':'uint256'},{'indexed':false,'name':'newState','type':'uint256'},{'indexed':false,'name':'time','type':'uint256'}],'name':'StateChanged','type':'event'}]";

        public static string BYTE_CODE = "0x6060604052610147806100136000396000f3006060604052600436106100405763ffffffff7c0100000000000000000000000000000000000000000000000000000000600035041663782a5c828114610042575b005b6100403373ffffffffffffffffffffffffffffffffffffffff167f0355e61ec6650dfdc23a9d941d34cabcbbab198479acc32511ce90a5192c81ac6003426040519182526040808301919091526060602083018190526007908301527f656e635261746500000000000000000000000000000000000000000000000000608083015260a0909101905180910390a27f4e783b715efde58d097be2fe2f2e2bc0bb1df07fd682aeab1d8b4dc063ffdd9a600060014260405180848152602001838152602001828152602001935050505060405180910390a15600a165627a7a7230582065aab2b2c4040d2eb012d4a2f5f1e9e209f837f718883d7728227563a005a0cb0029";
```

Declare now two classes corresponding to the "event" logs so they can be deserialised into a DTO (data transfer object),

ps: VS Code’s “Solidity” extension allows you to code generate those classes.

```csharp
[Event("BidAddedEventDTO")]
public class BidAddedEventDTO : IEventDTO
    {
        [Parameter("address", "sender", 1, true)]
        public string Sender { get; set; }

        [Parameter("uint256", "amount", 2, false)]
        public BigInteger Amount { get; set; }

        [Parameter("string", "encryptedRate", 3, false)]
        public string EncryptedRate { get; set; }

        [Parameter("uint256", "time", 4, false)]
        public BigInteger Time { get; set; }

    }

[Event("StateChangedEventDTO")]
    public class StateChangedEventDTO : IEventDTO
    {
        [Parameter("uint256", "currentState", 1, false)]
        public BigInteger CurrentState { get; set; }

        [Parameter("uint256", "newState", 2, false)]
        public BigInteger NewState { get; set; }

        [Parameter("uint256", "time", 3, false)]
        public BigInteger Time { get; set; }

    }
```

To send transactions, you will need to create a new Ethereum account and an instance of web3 using the account’s private key:

```csharp
var account = new Account("0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7");
var web3 = new Web3(account);
```

You are now ready to deploy your contract, using the DeployContract method and the compiled versions of your contract as payload.

```csharp
 var transactionReceipt = await web3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(ABI, BYTE_CODE, account.Address, new Nethereum.Hex.HexTypes.HexBigInteger(900000));
```

Using the “contract address” parameter returned by the transactionReceipt, you can now declare a **_contract_** variable that points to an instance of a contract object.

```csharp
var contract  = web3.Eth.GetContract(ABI, transactionReceipt.ContractAddress);
```

It's now possible to call functions within your contract using the GetFunction method.

```csharp
var addBidFunction = contract.GetFunction("addBid");
```

Before calling an Ethereum function, it’s good practice to evaluate the gas cost incurred by the call by using the **_EstimateGasAsync method_**.

```csharp
var gas = await addBidFunction.EstimateGasAsync(account.Address,new Nethereum.Hex.HexTypes.HexBigInteger(90000), null);
```

Finally, you can call your function, the **_transaction receipt_** will return information regarding the transaction that was sent.

```csharp
var addBidReceipt = await addBidFunction.SendTransactionAndWaitForReceiptAsync(account.Address,gas, null);
```

Calling a contract function on Ethereum won’t return the result. But you can use events to log state changes or function call results. In the case of our example, you can select the event “BidAdded” by calling the contract function **_GetEvent_**.

Event logs can be filtered for a specific block range or topics, in the example we are creating a filter that will return all the event logs for our recently deployed smart contract from the BlockNumber of adding the bid to the latest.

```csharp
var bidAddedEventLog = contract.GetEvent("BidAdded");
var filterInput =
                bidAddedEventLog.CreateFilterInput(new BlockParameter(addBidReceipt.BlockNumber), BlockParameter.CreateLatest());
var logs = await bidAddedEventLog.GetAllChanges<BidAddedEventDTO>(filterInput);
```

The same can be done with State Changes:

```csharp
 var stateChangedEventLog = contract.GetEvent("StateChanged");
            var filterInput2 =
                stateChangedEventLog.CreateFilterInput(new BlockParameter(addBidReceipt.BlockNumber), BlockParameter.CreateLatest());
            var logs2 = await stateChangedEventLog.GetAllChanges<StateChangedEventDTO>(filterInput2);
```