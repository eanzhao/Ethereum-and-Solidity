# What is Ethereum?

## Introduction

Goal of This Course:
- Build Web Apps with Ethereum.
- **Not** Trading cryptocurrencies.
- **Not** Super deep academic discussion of cryptocurrencies.

## A Short History Lesson

[Bitcoin: A Peer-to-Peer Electronic Cash System](https://bitcoin.org/bitcoin.pdf)

[Ethereum: The Ultimate Smart Contract and Decentralized Application Platform](http://web.archive.org/web/20131228111141/http://vbuterin.com/ethereum.html)
- DAC: Decentralized Autonomous Corporations.
- Smart Contracts. 

## What is Ethereum?

- 以太坊网络可以用于转移资产、存储数据。
- 有许多不同的以太坊网络。
  - 公共网络
  - 测试网络
  - 私有网络
- 以太坊网络由单个或多个节点形成（formed by）。
- 每个节点都是一个运行着以太坊客户端的机器。
- 任何人都可以运行一个节点。
- 每个节点都有区块链的完整数据（full copy）。
- 区块链是一个数据库，存储着过去曾经发生过的每一笔交易的记录。

## Interfacing with Ethereum Networks

For Developers: web3.js

For Consumers: Metamask, Mist Browser

## What's a Transaction?

Timeline:
- submit transaction
- Address sent to backend server
- Backend server used web3 library to create a `transaction` object
- Backend server sent `transaction` object to the ethereum network
- Backend server waited for transaction to be confirmed
- Backend server sent success message back to the browser

Transaction:
- nonce
  - How many times the sender has sent a transaction
- to
  - Address of account this money is going to
- value
  - Amount of ether to send to the target address
- gasPrice
  - Amount of ether the sender is willing to pay per unit gas to get the transaction processed
- startGas/gasLimit
  - Units of gas that this transaction can consume
- v, r, s
  - Cryptographic pieces of data that can be used to generate the senders account address. Generated from the sender's private key.

## Why'd We Wait?

Mining.

## Basic Blockchains

https://andersbrownworth.com/blockchain

Also see my code in `EAS.Section1` and `EAS.Section1.Tests`.

Main Idea of PoW:
```C#
public static BlockWithHash Mine(this BlockWithHash blockWithHash)
{
    if (blockWithHash.Hash.CheckDifficulty(Constants.Difficulty))
    {
        return blockWithHash;
    }

    while (!blockWithHash.Hash.CheckDifficulty(Constants.Difficulty))
    {
        blockWithHash.IncreaseNonce();
    }

    return blockWithHash;
}
```

## Block Time

根据全网算力调整挖矿难度。

https://etherscan.io/chart/blocktime

## Smart Contracts

Contract Account
- balance
  - Amount of ether this account owns.
- storage
  - Data storage for this contract.
- code
  - Raw machine code for this contract.

External Account
- Account Address. Public Key. Private Key.

Contract Source --(Deployment)-> Contract **Instance**

## The Solidity Programming Language

强类型语言。
类似JS。

Contract Definition (Solidity) -> Solidity Compiler -> 
- Byte code ready for deployment
- Application Binary Interface (ABI)

## Updated Remix Instructions for new UI

删掉Ballot.sol里的所有代码，然后在S中把编译器版本换为0.4.17

## Our First Contract

```solidity
pragma solidity ^0.4.17;

contract Inbox {
  string public message;
  
  function Inbox(string initialMessage) public {
    message = initialMessage;
  }
  
  function setMessage(string newMessage) public {
    message = newMessage;
  }
  
  function getMessage() public view returns (string) {
    return message;
  }
}
```

## Function Declarations

Common Function Types:
- public
  - Anyone can call this function.
- private
  - Only this contract can call this function.
- view
  - This function returns data and does not modify the contract's data.
- constant
  - This function returns data and does not modify the contract's data.
  - Same as `view`.
- pure
  - Function will not modify or even read the contract's data.
- payable
  - When someone call this function they might send ether along.

## Testing with Remix

In-Browser Fake Network. Remix VM.

## Behind the Scenes of Deployment

Leave the `to` field as blank.

Transaction for creating contract:
- nonce
  - How many times the sender has sent a transaction
- to
  - empty
- data
  - Compiled bytecode of the contract
- value
  - Amount of `Wei` to send to the target address
- gasPrice
  - Amount of Wei the sender is willing to pay per unit gas to get the transaction processed
- startGas/gasLimit
  - Units of gas that this transaction can consume
- v, r, s
  - Cryptographic pieces of data that can be used to generate the senders account address. Generated from the sender's private key.

## More on Running Functions Than You Want to Know

Changing Anything on the blockchain? -> Submit a transaction.

Two ways of Running Contract Functions:
- `Calling` a function
  - Cannot modify the contract's data
  - Can return data
  - Runs instantly
  - Free to do!
- Sending a Transaction to a Function
  - Can modify a contract's data
  - Takes time to execute!
  - Returns the transaction hash
  - Costs money!

## Wei vs Ether

https://eth-converter.com/

## Gas and Transactions

https://www.evm.codes/
- MINIMUM GAS

gasPrice
- Amount of Wei the sender is willing to pay per unit gas to get this transaction processed.

startGas/gasLimit
- Units of gas that this transaction can consume.

## Mnemonic Phrases

https://iancoleman.io/bip39/

## Obtaining More Test Ether from Recommended Faucet

About to switch to Sepolia Network:
https://sepoliafaucet.com/