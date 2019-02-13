---
uti: com.xamarin.workbook
id: d0548e89-9a34-48a1-91c9-45f5bb32a816
title: nethereum-events-gettingstarted
platforms:
- Console
packages:
- id: Nethereum.Web3
  version: 3.0.0
---
# Getting started with events

## Pre-requisites:

This document is a Workbook, find more about workbooks' installation requirements  [here](https://docs.microsoft.com/en-us/xamarin/tools/workbooks/install).

Download the test chain matching your environment from https://github.com/Nethereum/Testchains

Start a Geth chain ( **geth-clique-linux\_** **_geth-clique-windows_** **_geth-clique-mac_**) using startgeth.bat (Windows) or startgeth.sh (Mac/Linux). The chain is setup with the Proof of Authority consensus and will start the mining process immediately.

## Quick introduction to Events, Logs and Filters

Events in smart contracts write data to the transaction receipt logs, providing a way to get extra information about a smart contract transactions.

A very good example is the “Transfer” event in the ERC20 Standard Token contract. Everytime that a token transfer has ocurred, an event gets logged providing information of the “sender”, “receiver” and the token amount. In this scenario we are only interested in the Deployment, Transfer function and Transfer Event of the ERC20 smart contract.

![Transfer and transfer event](https://github.com/Nethereum/Nethereum.Workbooks/raw/master/docs/screenshots/TransferEvent.png)
Above we can see, the event declaration with the different indexed parameters, these will allow us later on to “filter” for specific events. For example ,“Transfer” events for a specific receiver address “\_to”.

The Transfer event can be seen in the function prefixed with the “emit” keyword.

```csharp
#r "Nethereum.Web3"
```

```csharp
#r "Nethereum.Accounts"
```

```csharp
using Nethereum.Web3;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts.CQS;
using Nethereum.Util;
using Nethereum.Web3.Accounts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Contracts;
using Nethereum.Contracts.Extensions;
using System.Numerics;
```

To deploy a contract we will create a class inheriting from the ContractDeploymentMessage, here we can include our compiled byte code and other constructor parameters.

As we can see below the StandardToken deployment message includes the compiled bytecode of the ERC20 smart contract and the constructor parameter with the “totalSupply” of tokens.

Each parameter is described with an attribute Parameter, including its name "totalSupply", type "uint256" and order.

```csharp
public class StandardTokenDeployment : ContractDeploymentMessage
{

            public static string BYTECODE = "0x60606040526040516020806106f5833981016040528080519060200190919050505b80600160005060003373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005081905550806000600050819055505b506106868061006f6000396000f360606040523615610074576000357c010000000000000000000000000000000000000000000000000000000090048063095ea7b31461008157806318160ddd146100b657806323b872dd146100d957806370a0823114610117578063a9059cbb14610143578063dd62ed3e1461017857610074565b61007f5b610002565b565b005b6100a060048080359060200190919080359060200190919050506101ad565b6040518082815260200191505060405180910390f35b6100c36004805050610674565b6040518082815260200191505060405180910390f35b6101016004808035906020019091908035906020019091908035906020019091905050610281565b6040518082815260200191505060405180910390f35b61012d600480803590602001909190505061048d565b6040518082815260200191505060405180910390f35b61016260048080359060200190919080359060200190919050506104cb565b6040518082815260200191505060405180910390f35b610197600480803590602001909190803590602001909190505061060b565b6040518082815260200191505060405180910390f35b600081600260005060003373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060008573ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600050819055508273ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff167f8c5be1e5ebec7d5bd14f71427d1e84f3dd0314c0f7b2291e5b200ac8c7c3b925846040518082815260200191505060405180910390a36001905061027b565b92915050565b600081600160005060008673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600050541015801561031b575081600260005060008673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060003373ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000505410155b80156103275750600082115b1561047c5781600160005060008573ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000828282505401925050819055508273ffffffffffffffffffffffffffffffffffffffff168473ffffffffffffffffffffffffffffffffffffffff167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef846040518082815260200191505060405180910390a381600160005060008673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008282825054039250508190555081600260005060008673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060003373ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000828282505403925050819055506001905061048656610485565b60009050610486565b5b9392505050565b6000600160005060008373ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000505490506104c6565b919050565b600081600160005060003373ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600050541015801561050c5750600082115b156105fb5781600160005060003373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008282825054039250508190555081600160005060008573ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000828282505401925050819055508273ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef846040518082815260200191505060405180910390a36001905061060556610604565b60009050610605565b5b92915050565b6000600260005060008473ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060008373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005054905061066e565b92915050565b60006000600050549050610683565b9056";

    public StandardTokenDeployment() : base(BYTECODE){}

    [Parameter("uint256", "totalSupply")]
    public BigInteger TotalSupply { get; set; }
}
```

We can call the functions of smart contract to query the state of a smart contract or do any computation, which will not affect the state of the blockchain.

To do so we will need to create a class which inherits from "FunctionMessage". First we will decorate the class with a "Function" attribute, including the name and return type.

Each parameter of the the function will be a property of the class, each of them decorated with the "Parameter" attribute, including the smart contract name, type and parameter order.

For the ERC20 smart contract, the "balanceOf" function definition, provides the query interface to get the token balance of a given address. As we can see this function includes only one parameter "\_owner", of the type "address".

```csharp
[Function("balanceOf", "uint256")]
public class BalanceOfFunction : FunctionMessage
{
    [Parameter("address", "_owner", 1)]
    public string Owner { get; set; }
}
```

Another type of smart contract function will be correspondent to a transaction that will change the state of the smart contract (or smart contracts).

For example The "transfer" function definition for the ERC20 smart contract, includes the parameters “\_to” address parameter as a string, and the “\_value” or TokenAmount we want to transfer.

In a similar way to the "balanceOf" function, all the parameters include the solidity type, parameter name and parameter order.

Note: When working with functions, it is very important to have the parameters types, and function name correct as all of these make the signature of the function.

```csharp
[Function("transfer", "bool")]
public class TransferFunction : FunctionMessage
{
    [Parameter("address", "_to", 1)]
    public string To { get; set; }

    [Parameter("uint256", "_value", 2)]
    public BigInteger TokenAmount { get; set; }
}
```

Finally smart contracts also have events. Events in smart contracts write the blockchain log, providing a way to retrieve further information of any smart contract interaction occurred.

To create an Event definition, we need to create a class that inherits from IEventDTO, decorated with the Event attribute.

The Transfer Event, similar to the Function it also includes the parameters with the name, order and type. But also a boolean value indicating if the parameter is indexed or not.

Indexed parameters will allow us later on to query the blockchain for those values.

```csharp
[Event("Transfer")]
public class TransferEventDTO : IEventDTO
{
    [Parameter("address", "_from", 1, true)]
    public string From { get; set; }

    [Parameter("address", "_to", 2, true)]
    public string To { get; set; }

    [Parameter("uint256", "_value", 3, false)]
    public BigInteger Value { get; set; }
}
```

### Instantiating Web3 and the Account

A simple way to run this sample is to use one of the pre-configured private chains which can be found https://github.com/Nethereum/TestChains (Geth, Parity, Ganache) using the Account “0x12890d2cce102216644c59daE5baed380d84830c” with private key “0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7“, or alternatively use your own testchain with your own account / private key.

To create an instance of web3 we first provide the url of our testchain and the private key of our account. When providing an Account instantiated with a  private key all our transactions will be signed “offline” by Nethereum.

```csharp
var url = "http://localhost:8545";
var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
var account = new Account(privateKey);
var web3 = new Web3(account, url);
```

### Deploying the Contract

The next step is to deploy our Standard Token ERC20 smart contract, in this scenario the total supply (number of tokens) is going to be 100,000.

First we create an instance of the StandardTokenDeployment with the TotalSupply amount.

```csharp
var deploymentMessage = new StandardTokenDeployment
{
    TotalSupply = 100000
};
```

Then we create a deployment handler using our contract deployment definition and simply deploy the contract using the deployment message. We are auto estimating the gas, getting the latest gas price and nonce so nothing else is set anything on the deployment message.

Finally, we wait for the deployment transaction to be mined, and retrieve the contract address of the new contract from the receipt.

```csharp
var deploymentHandler = web3.Eth.GetContractDeploymentHandler<StandardTokenDeployment>();
var transactionReceipt = await deploymentHandler.SendRequestAndWaitForReceiptAsync(deploymentMessage);
var contractAddress = transactionReceipt.ContractAddress;
```

### Transfer

Once we have deployed the contract, we can execute our first transfer transaction. The transfer function will write to the log the transfer event.

First we can create a TransactionHandler using the TrasferFunction definition and a TransferFunction message including the “receiverAddress” and the amount of tokens we want to send.

Finally do the transaction transfer and wait for the receipt to be “mined” and included in the blockchain.

```csharp
var receiverAddress = "0xde0B295669a9FD93d5F28D9Ec85E40f4cb697BAe";
var transferHandler = web3.Eth.GetContractTransactionHandler<TransferFunction>();
var transfer = new TransferFunction()
{
    To = receiverAddress,
    TokenAmount = 100
};
var transactionReceipt2 = await transferHandler.SendRequestAndWaitForReceiptAsync(contractAddress, transfer);
```

## Decoding the Event from the TransactionReceipt

Event logs are part of the TransactionReceipts, so using the Transaction receipt from the previous transfer we can decode the TransferEvent using the extension method “DecodeAllEvents<TransferEventDTO>()”.

Note that this method returns an array of Decoded Transfer Events as opposed to a single value, because the receipt can include more than one event of the same signature.

```csharp
var transferEventOutput = transactionReceipt2.DecodeAllEvents<TransferEventDTO>();
```

## Contract Filters and Event Logs

Another way to access the event logs of a smart contract is to either get all changes of the logs (providing a filter message) or create filters and retrieve changes which apply to our filter message periodically.\
\
To access the logs, first of all, we need to create a transfer event handler for our contract address, and Evend definition.(TransferEventDTO).

```csharp
var transferEventHandler = web3.Eth.GetEvent<TransferEventDTO>(contractAddress);
```

Using the event handler, we can create a filter message for our transfer event using the default values.

The default values for BlockParameters are Earliest and Latest, so when we retrieve the logs we will get all the transfer events from the first block to the latest block of this contract.

```csharp
var filterAllTransferEventsForContract = transferEventHandler.CreateFilterInput();
```

Once we have created the message we can retrieve all the logs using the event and GetAllChanges. In this scenario, because we have made only one transfer, we will have only one Transfer Event.

```csharp
var allTransferEventsForContract = await transferEventHandler.GetAllChanges(filterAllTransferEventsForContract);
```

If we now make another Transfer to a different address

```csharp
var receiverAddress2 = "0x3e0B295669a9FD93d5F28D9Ec85E40f4cb697BAe";
var transfer2 = new TransferFunction()
{
    To = receiverAddress2,
    TokenAmount = 1000
};
var transactionReceipt3 = await transferHandler.SendRequestAndWaitForReceiptAsync(contractAddress, transfer2);
```

Using the same filter input message and making another GetAllChanges call, we will now get the two Transfer Event logs.

```csharp
var allTransferEventsForContract2 = await transferEventHandler.GetAllChanges(filterAllTransferEventsForContract);
```

Filter messages can limit the results (similar to block ranges) to the indexed parameters,  for example we can create a filter for only our sender address AND the receiver address. As a reminder our Event has as indexed parameters the “\_from” address and “\_to” address.

```csharp
var filterTransferEventsForContractReceiverAddress2 = transferEventHandler.CreateFilterInput(account.Address, receiverAddress2);
var transferEventsForContractReceiverAddress2 = await transferEventHandler.GetAllChanges(filterTransferEventsForContractReceiverAddress2);
```

The order the filter values is based on the event parameters order, if we want to include all the transfers to the “receiverAddress2”, the account address from will need to be set to null to be ignored.

Note: We are using the array format to allow for null input of the first parameter.

```csharp
var filterTransferEventsForContractAllReceiverAddress2 = transferEventHandler.CreateFilterInput(null, new []{receiverAddress2});
var transferEventsForContractAllReceiverAddress2 = await transferEventHandler.GetAllChanges(filterTransferEventsForContractAllReceiverAddress2);
```

Another scenario is when you want to include multiple indexed values, for example transfers for “receiverAddress1” OR “receiverAddress2”. Then you will need to use an array of the values you are interested.

```csharp
var filterTransferEventsForContractAllReceiverAddresses = transferEventHandler.CreateFilterInput(null, new []{receiverAddress2, receiverAddress});
var transferEventsForContractAllReceiverAddresses = await transferEventHandler.GetAllChanges(filterTransferEventsForContractAllReceiverAddresses);
```

### Creating filters to retrieve periodic changes

Another option is to create filters that return only the changes occurred since we got the previous results. This eliminates the need of tracking the last block the events were checked and delegate this to the Ethereum client.

Using the same filter message we created before we can create the filter and get the filterId.

```csharp
var filterIdTransferEventsForContractAllReceiverAddress2  = await transferEventHandler.CreateFilterAsync(filterTransferEventsForContractAllReceiverAddress2);
```

One thing to note, if  try to get the filter changes now, we will not get any results because the filter only returns the changes since creation.

```csharp
var result = await transferEventHandler.GetFilterChanges(filterIdTransferEventsForContractAllReceiverAddress2);
```

But, if we make another transfer using the same values

```csharp
await transferHandler.SendRequestAndWaitForReceiptAsync(contractAddress, transfer2);
```

and execute get filter changes using the same filter id, we will get the event for the previous transfer.

```csharp
var result2 = await transferEventHandler.GetFilterChanges(filterIdTransferEventsForContractAllReceiverAddress2);
```

Executing the same again will return no results because no new transfers have occurred since the last execution of GetFilterChanges.

```csharp
var result3 = await transferEventHandler.GetFilterChanges(filterIdTransferEventsForContractAllReceiverAddress2);
```

## Events for all Contracts

Different contracts can have and raise/log the same event with the same signature, a simple example is the multiple standard token ERC20 smart contracts that are part of Ethereum. There might be scenarios you want to capture all the Events for different contracts using a specific filter, for example all the transfers to an address.

In Nethereum creating an Event (handler) without a contract address allows to create filters which are not attached to a specific contract.

```csharp
var transferEventHandlerAnyContract = web3.Eth.GetEvent<TransferEventDTO>();
```

There is already a contract deployed in the chain, from the previous sample,  so to demonstrate the access to events of multiple contracts we can deploy another standard token contract.

```csharp
var transactionReceiptNewContract = await deploymentHandler.SendRequestAndWaitForReceiptAsync(deploymentMessage);
var contractAddress2 = transactionReceiptNewContract.ContractAddress;
```

and make another transfer using this new contract and the same receiver address.

```csharp
await transferHandler.SendRequestAndWaitForReceiptAsync(contractAddress2, transfer);
```

Creating a default filter input, and getting all changes, will retrieve all the transfer events for all contracts.

```csharp
var filterAllTransferEventsForAllContracts = transferEventHandlerAnyContract.CreateFilterInput();
var allTransferEventsForContract3 = await transferEventHandlerAnyContract.GetAllChanges(filterAllTransferEventsForAllContracts);
```

If we want to retrieve only all the transfers to the “receiverAddress”,  we can create the same filter as before ,including only the second indexed parameter (“to”). This will return the Transfers only to this address for both contracts.

```csharp
var filterTransferEventsForAllContractsReceiverAddress2 = transferEventHandlerAnyContract.CreateFilterInput(null, new[]{receiverAddress});
await transferEventHandlerAnyContract.GetAllChanges(filterTransferEventsForAllContractsReceiverAddress2);
```