# Advanced Smart Contracts

## The Lottery Contract

Lottery Contract:
- Prize Pool
- Players

Roles:
- Manager
- Player 1
- Player 2

## Lottery Design

Variables:
- manager
  - Address of person who created the contract.
- players
  - Array of addresses of people who have entered.

Functions:
- enter
  - Enters a player into the lottery.
- pickWinner
  - Randomly picks a winner and sends them the prize pool.

## Basic Solidity Types

Basic Types
- string
- bool
- int
- uint
- fixed/ufixed
- address

Integer
- int8
- int16
- int32
- int64
- int128
- int256

Unsigned Integer
- uint8
- uint16
- uint32
- uint64
- uint128
- uint256

## Starting the Lottery Contract

```solidity
pragma solidity ^0.4.17;

contract Lottery {
  address public manager;
  address[] public players;

  function Lottery() public {
    manager = msg.sender;
  }
}
```

## The Message Global Variable

The `msg` Global Variable:
- msg.data
  - `Data` filed from the call or transaction that invoked the current function.
- msg.gas
  - Amount of gas the current function invocation has available.
- msg.sender
  - Address of the account that started the current function invocation.
- msg.value
  - Amount of ether (in wei) that was sent along with the function invocation.

## Overview of Arrays & Mappings & Structs

Reference Types:
- fixed array
  - Array that contains a single type of element. Has an unchanging length.
  - int[3]
  - bool[2]
- dynamic array
  - Array that contains a single type of element. Can change in size over time.
  - int[]
  - bool[]
- mapping
  - Collection of key-value pairs. Think of Javascript objects, Ruby hashes, or Python dictionary. All keys must be of the same type, and all values must be of the same type.
  - mapping(string => string)
  - mapping(int => bool)
- struct
  - Collection of key-value pairs that can have different types.
```solidity
      struct Car {
        string make;
        string model;
        uint value;
      }
```

## Big Solidity Gotcha

```solidity
const myArray = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
];
```

solidity可以构造二维数组，但是js不行。
是语言之间communication问题。

## Entering the Lottery

```solidity
pragma solidity ^0.4.17;

contract Lottery {
  address public manager;
  address[] public players;

  function Lottery() public {
    manager = msg.sender;
  }

  function enter() public payable {
      players.push(msg.sender);
  }
}
```

## Validation with Require Statements

`require`

```solidity
pragma solidity ^0.4.17;

contract Lottery {
  address public manager;
  address[] public players;

  function Lottery() public {
    manager = msg.sender;
  }

  function enter() public payable {
    require(msg.value > .01 ether);
    players.push(msg.sender);
  }
}
```

## Pseudo Random Number Generator

Current block difficulty + Current time + Addresses of players

-> SHA3 Algorithm

-> Really big number

```solidity
pragma solidity ^0.4.17;

contract Lottery {
  address public manager;
  address[] public players;

  function Lottery() public {
    manager = msg.sender;
  }

  function enter() public payable {
    require(msg.value > .01 ether);
    players.push(msg.sender);
  }

  function random() private view returns (uint) {
    return uint(keccak256(block.difficulty, now, players));
  }
}
```

## Selecting a Winner & Sending Ether from Contracts

```solidity
pragma solidity ^0.4.17;

contract Lottery {
  address public manager;
  address[] public players;

  function Lottery() public {
    manager = msg.sender;
  }

  function enter() public payable {
    require(msg.value > .01 ether);
    players.push(msg.sender);
  }

  function random() private view returns (uint) {
    return uint(keccak256(block.difficulty, now, players));
  }

  function pickWinner() public {
    uint index = random() % players.length;
    players[index].transfer(this.balance);
    players = new address[](0);
  }
}
```

## Resetting Contract State



## Requiring Managers & Function Modifiers

```solidity
pragma solidity ^0.4.17;

contract Lottery {
    address public manager;
    address[] public players;

    function Lottery() public {
        manager = msg.sender;
    }

    function enter() public payable {
        require(msg.value > .01 ether);
        players.push(msg.sender);
    }

    function random() private view returns (uint) {
        return uint(keccak256(block.difficulty, now, players));
    }

    function pickWinner() public restricted {
        uint index = random() % players.length;
        players[index].transfer(this.balance);
        players = new address[](0);
    }

    modifier restricted() {
        require(msg.sender == manager);
        _;
    }
}
```

## Returning Players Array

```solidity
pragma solidity ^0.4.17;

contract Lottery {
    address public manager;
    address[] public players;

    function Lottery() public {
        manager = msg.sender;
    }

    function enter() public payable {
        require(msg.value > .01 ether);
        players.push(msg.sender);
    }

    function random() private view returns (uint) {
        return uint(keccak256(block.difficulty, now, players));
    }

    function pickWinner() public restricted {
        uint index = random() % players.length;
        players[index].transfer(this.balance);
        players = new address[](0);
    }

    modifier restricted() {
        require(msg.sender == manager);
        _;
    }

    function getPlayers() public view returns (address[]) {
        return players;
    }
}
```