---
uti: com.xamarin.workbook
id: 701d2cce-38a5-43b6-a682-e7684c44d480
title: nethereum-gettingstarted-smartcontracts.workbook
platforms:
- Console
packages:
- id: Nethereum.Accounts
  version: 3.0.0
- id: Nethereum.Web3
  version: 3.0.0
---

# Quick introduction to smart contracts integration with Nethereum

This document is a Workbook, find more about workbooks' installation requirements  [here](https://docs.microsoft.com/en-us/xamarin/tools/workbooks/install).

The purpose of this sample is the following:

* Creating an account using a private key

* Deploying a smart contract (the sample provided is the standard ERC20 token contract)

* Estimating the gas cost of a contract transaction

* Sending a transaction to the smart contract (in this scenario transfering balance)

* Making a call to a smart contract (in this scenario get the balance of an account)

* Retrieving the state of a smart contract from a previous block

## Prerequisites:

First, let's download the test chain matching your environment from <https://github.com/Nethereum/Testchains>

Start a Geth chain (geth-clique-linux\\, geth-clique-windows\\ or geth-clique-mac\\) using **startgeth.bat** (Windows) or **startgeth.sh** (Mac/Linux). The chain is setup with the Proof of Authority consensus and will start the mining process immediately.

```csharp
#r "Nethereum.Web3"
```

```csharp
#r "Nethereum.Accounts"
```

## The sample

First off we will add the using statement to Nethereum.Web3. All other namespaces will be included directly in the sample.

```csharp
using Nethereum.Web3; using Nethereum.Web3.Accounts;
```

### Byte code and ABI

The next step is to declare the contract byte code and abi for the contrat.

This is a standard token contract which can be found here: <https://github.com/Nethereum/Nethereum.Workbooks/blob/master/StandardToken.sol>.

The contract includes an initial supply of tokens when deployed, and capability to transfer amounts between accounts and retrieve the balance.

The bytecode represents the compiled contract, and the abi represents the interface definition with the contract.

We will use this to deploy this contract to the Ethereum chain and interact with it.

```csharp
 var contractByteCode = "0x60606040526040516020806106f5833981016040528080519060200190919050505b80600160005060003373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005081905550806000600050819055505b506106868061006f6000396000f360606040523615610074576000357c010000000000000000000000000000000000000000000000000000000090048063095ea7b31461008157806318160ddd146100b657806323b872dd146100d957806370a0823114610117578063a9059cbb14610143578063dd62ed3e1461017857610074565b61007f5b610002565b565b005b6100a060048080359060200190919080359060200190919050506101ad565b6040518082815260200191505060405180910390f35b6100c36004805050610674565b6040518082815260200191505060405180910390f35b6101016004808035906020019091908035906020019091908035906020019091905050610281565b6040518082815260200191505060405180910390f35b61012d600480803590602001909190505061048d565b6040518082815260200191505060405180910390f35b61016260048080359060200190919080359060200190919050506104cb565b6040518082815260200191505060405180910390f35b610197600480803590602001909190803590602001909190505061060b565b6040518082815260200191505060405180910390f35b600081600260005060003373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060008573ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600050819055508273ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff167f8c5be1e5ebec7d5bd14f71427d1e84f3dd0314c0f7b2291e5b200ac8c7c3b925846040518082815260200191505060405180910390a36001905061027b565b92915050565b600081600160005060008673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600050541015801561031b575081600260005060008673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060003373ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000505410155b80156103275750600082115b1561047c5781600160005060008573ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000828282505401925050819055508273ffffffffffffffffffffffffffffffffffffffff168473ffffffffffffffffffffffffffffffffffffffff167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef846040518082815260200191505060405180910390a381600160005060008673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008282825054039250508190555081600260005060008673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060003373ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000828282505403925050819055506001905061048656610485565b60009050610486565b5b9392505050565b6000600160005060008373ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000505490506104c6565b919050565b600081600160005060003373ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600050541015801561050c5750600082115b156105fb5781600160005060003373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008282825054039250508190555081600160005060008573ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000828282505401925050819055508273ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef846040518082815260200191505060405180910390a36001905061060556610604565b60009050610605565b5b92915050565b6000600260005060008473ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060008373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005054905061066e565b92915050565b60006000600050549050610683565b9056";
 var abi = @"[{""constant"":false,""inputs"":[{""name"":""_spender"",""type"":""address""},{""name"":""_value"",""type"":""uint256""}],""name"":""approve"",""outputs"":[{""name"":""success"",""type"":""bool""}],""type"":""function""},{""constant"":true,""inputs"":[],""name"":""totalSupply"",""outputs"":[{""name"":""supply"",""type"":""uint256""}],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_from"",""type"":""address""},{""name"":""_to"",""type"":""address""},{""name"":""_value"",""type"":""uint256""}],""name"":""transferFrom"",""outputs"":[{""name"":""success"",""type"":""bool""}],""type"":""function""},{""constant"":true,""inputs"":[{""name"":""_owner"",""type"":""address""}],""name"":""balanceOf"",""outputs"":[{""name"":""balance"",""type"":""uint256""}],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_to"",""type"":""address""},{""name"":""_value"",""type"":""uint256""}],""name"":""transfer"",""outputs"":[{""name"":""success"",""type"":""bool""}],""type"":""function""},{""constant"":true,""inputs"":[{""name"":""_owner"",""type"":""address""},{""name"":""_spender"",""type"":""address""}],""name"":""allowance"",""outputs"":[{""name"":""remaining"",""type"":""uint256""}],""type"":""function""},{""inputs"":[{""name"":""_initialAmount"",""type"":""uint256""}],""type"":""constructor""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""_from"",""type"":""address""},{""indexed"":true,""name"":""_to"",""type"":""address""},{""indexed"":false,""name"":""_value"",""type"":""uint256""}],""name"":""Transfer"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""_owner"",""type"":""address""},{""indexed"":true,""name"":""_spender"",""type"":""address""},{""indexed"":false,""name"":""_value"",""type"":""uint256""}],""name"":""Approval"",""type"":""event""}]";
```

### The account address and private key

First of all we will need a private key to be able to sign our transactions, the ethereum address is calculated from this private key, so each transaction or message signed with this private key can be related to the ethereum address / account. The private chain has already been configured with the address "0x12890d2cce102216644c59daE5baed380d84830c” containing some Ether. The private key for this address is "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7”.

We can create an instance of an Account using its private key , which will then be used to sign the transactions in the background.

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

### Deploying Smart contracts

In this example, we are going to use the standard token smart contract. This smart contract is initialized on deployment with the “totalSupply” of tokens. As we can see in the code below, the total balance is assigned to the sender (the address that deployed the contract)

function Standard\_Token(uint256 \_initialAmount) {
        balances\[msg.sender] = \_initialAmount;
        \_totalSupply = \_initialAmount;
    }

When deploying the smart contract using Nethereum, this constructor parameter is send alongside the byteCode as follows:

```csharp
 System.Numerics.BigInteger totalSupply = System.Numerics.BigInteger.Parse("1000000000000000000");
 var receipt = await web3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(abi, contractByteCode, senderAddress,  new Nethereum.Hex.HexTypes.HexBigInteger(900000), null, totalSupply);
```

The method used to deploy the contract, first sends the transaction, then waits for the transaction to be mined (included in the chain's next block) and finally returns the transaction receipt.

The transaction receipt of a newly deployed contract includes the address of the contract. From now on we will use this address to interact with the smart contract.

### Interacting with the Contract

To interact with a contract we need its address and its ABI. The ABI provides the interface definition, including all the information about the different function calls, returned values and events. The ABI is necessary to convert the .Net types and values to the Ethereum format.

```csharp
 var contract = web3.Eth.GetContract(abi, receipt.ContractAddress);
```

Once we have created an instance of a contract, we can use specific functions to interact with the smart contract. These can be retrieved using their name.

```csharp
var transferFunction = contract.GetFunction("transfer");
var balanceFunction = contract.GetFunction("balanceOf");
```

### Transfering some tokens

With a function, we can start interacting with the smart contract. One of the functions in our deployed contract is the "transfer" function, which is used to transfer a token between 2 parties.

**function transfer(address \_to, uint256 \_value) returns (bool success) {…}**

To interact with this function using Nethereum, as we can see on the solidity example, we will need the address that we want to send the tokens ( \_**to**) and the amount ( **\_value**) to send.

```csharp
var newAddress = "0xde0B295669a9FD93d5F28D9Ec85E40f4cb697BAe";
```

```csharp
var amountToSend = 1000;
```

Before we execute the transaction, we can “estimate” how much the transaction will cost in gas, by simulating it. When interacting with smart contracts, each instruction and / or storage has some cost. Each cost unit is represented as a gas unit. If we supply too much gas, the gas that has not been spent will be returned, but if an error occurred whilts processing the transaction all the gas will be consumed.

To estimate the gas cost in our transfer function we can call \*\*EstimateGasAsync \*\*passing the same parameters.

```csharp
var gas = await transferFunction.EstimateGasAsync(senderAddress, null, null, newAddress, amountToSend);
```

Now we have the “estimated gas”, we can use that value as one of the parameters for the transaction.

```csharp
var receiptFirstAmountSend = await transferFunction.SendTransactionAndWaitForReceiptAsync(senderAddress, gas, null, null, newAddress, amountToSend);
```

Using the method **SendTransactionAndWaitForReceiptAsync**, we wait for the transaction to be mined and included in the chain.

Once it is included in the chain, we are able to query the state of the smart contract. For example, we can now match the balance of the address to the amount sent using the balanceFunction of the smart contract.

function balanceOf(address \_owner) constant returns (uint256 balance) {
        return balances\[\_owner];
}

```csharp
 var balanceFirstAmountSend = await balanceFunction.CallAsync<int>(newAddress);
```

Repeating the exercise and sending the same amount.

```csharp
var receiptSecondAmountSend = await transferFunction.SendTransactionAndWaitForReceiptAsync(senderAddress, gas, null, null, newAddress, amountToSend);
```

```csharp
var transactionHashSecondAmountSend = receiptSecondAmountSend.TransactionHash;
```
When the transaction has been included in the next block, if we check the balance again, we will see how it has doubled.

```csharp
 var balanceSecondAmountSend = await balanceFunction.CallAsync<int>(newAddress);
```

To query the state from a previous block, we can include the blockNumber as one of the parameters. Here, we are retrieving the balance for the first transaction using the block number included in the receipt.

```csharp
var originalBalanceFirstAmoundSend = await balanceFunction.CallAsync<int>(new Nethereum.RPC.Eth.DTOs.BlockParameter(receiptFirstAmountSend.BlockNumber), newAddress);
```
