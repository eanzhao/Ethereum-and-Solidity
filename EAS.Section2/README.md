# Smart Contracts with Solidity

## Project Requirements

Boilerplate Design

- Issue: 想在JS项目里写Solidity
- Solution: Set up the Solidity compiler to build our contracts.


- Issue: 想要快速测试合约
- Solution: Set up a custom Mocha test runner that can somehow test Solidity code.


- Issue: 想把合约部署到公开网络
- Solution: Set up a deploy script to compile + deploy our contract.

## Compiling Solidity
```js
const path = require('path');
const fs = require('fs');
const solc = require('solc');

const inboxPath = pash.resolve(__dirname, 'contracts', 'Index.sol');
const source = fs.readFileSync(inboxPath, "utf8");

console.log(solc.compile(source, 1));
```

## The Compile Script
`node compile.js`

## Installing Modules
`npm install mocha ganache-cli web3`

```js
// Inbox.test.js
const assert = require('assert');
const ganache = require('ganache');
const { Web3 } = require('web3');
```

## Web3 Versioning

- v0.x.x
  - "Primitive" interface - only callbacks for async code
- v1.x.x
  - Support for promises + async/await

## Web3 Providers

Web3 -> **web3**

Ganache -> Provider (as a cellphone) -> **web3**

Provider is exchangeable.

```js
// Inbox.test.js
const assert = require('assert');
const ganache = require('ganache');
const { Web3 } = require('web3');
const web3 = new Web3(ganache.provider());
```

## Testing with Mocha

Mocha Functions
- it
  - Run a test and make an assertion.
- describe
  - Groups together `it` functions.
- beforeEach
  - Execute some general setup code.

`npm run test`

```js
const assert = require('assert');

class Car {
  park() {
    return 'stopped';
  }
  drive() {
    return 'vroom';
  }
}

let car;

beforeEach(() => {
  car = new Car();
});

describe('Car', () => {
  it('can park', () => {
    assert.strictEqual(car.park(), 'stopped');
  });
  it('can drive', () => {
    assert.strictEqual(car.drive(), 'vroom');
  });
});
```

## Mocha Structure

Mocha Starts 

-> Deploy a new contract (beforeEach)

-> Manipulate the contract (it)

-> Make an assertion about the contract (it)

-> Deploy a fresh new contract 

-> ...

## Fetching Accounts from Ganache

```js
const assert = require('assert');
const ganache = require('ganache');
const { Web3 } = require('web3');
const web3 = new Web3(ganache.provider());

beforeEach(() => {
  // Get a list of all accounts
  web3.eth.getAccounts().then((fetchedAccounts) => {
    console.log(fetchedAccounts);
  });
});

describe("Inbox", () => {
  it("deploys a contract", () => {});
});
```

## Refactor to Async/Await

```js
const assert = require('assert');
const ganache = require('ganache');
const { Web3 } = require('web3');
const web3 = new Web3(ganache.provider());

let accounts;

beforeEach(async () => {
  // Get a list of all accounts
  accounts = await web3.eth.getAccounts();
});

describe("Inbox", () => {
  it("deploys a contract", () => {
    console.log(accounts);
  });
});
```

## Deployment with Web3

```js
const assert = require('assert');
const ganache = require('ganache');
const { Web3 } = require('web3');
const web3 = new Web3(ganache.provider());
const { interface, bytecode } = require("../compile");

let accounts;
let inbox;

beforeEach(async () => {
  // Get a list of all accounts
  accounts = await web3.eth.getAccounts();
  inbox = await new web3.eth.Contract(JSON.parse(interface))
    .deploy({
      data: bytecode,
      arguments: ["Hi there!"],
    })
    .send({ from: accounts[0], gas: "1000000" });
});

describe("Inbox", () => {
  it("deploys a contract", () => {
    console.log(inbox);
  });
});
```

## Deployed Inbox Overview

`new web3.eth.Contract(JSON.parse(interface))`: Teaches web3 about what methods an Inbox contract has.

`.deploy({ data: bytecode, arguments: ['Hi there'] })`: Tells web3 that we want to deploy a new copy of this contract.

`.send({ from: accounts[0], gas: 1000000' })`: Instructs web3 to send out a transaction that creates this contract.

## Testing Message Updates

```js
const assert = require('assert');
const ganache = require('ganache');
const { Web3 } = require('web3');
const web3 = new Web3(ganache.provider());
const { interface, bytecode } = require("../compile");

let accounts;
let inbox;

beforeEach(async () => {
  // Get a list of all accounts
  accounts = await web3.eth.getAccounts();
  inbox = await new web3.eth.Contract(JSON.parse(interface))
          .deploy({
            data: bytecode,
            arguments: ["Hi there!"],
          })
          .send({ from: accounts[0], gas: "1000000" });
});

describe("Inbox", () => {
  it("deploys a contract", () => {
    assert.ok(inbox.options.address);
  });
  it("has a default message", async () => {
    const message = await inbox.methods.message().call();
    assert.strictEqual(message, "Hi there!");
  });
  it("can change the message", async () => {
    await inbox.methods.setMessage("bye").send({ from: accounts[0] });
    const message = await inbox.methods.message().call();
    assert.strictEqual(message, "bye");
  });
});
```

## Deployment with Infura

Ethereum Network -> Infura API -> Provider -> web3

## Wallet Provider Setup

`npm install @truffle/hdwallet-provider`

```js
// deploy.js
const HDWalletProvider = require('@truffle/hdwallet-provider');
const { Web3 } = require('web3');
const { interface, bytecode } = require('./compile');

const provider = new HDWalletProvider(
        'REPLACE_WITH_YOUR_MNEMONIC',
        'REPLACE_WITH_YOUR_INFURA_URL'
);
const web3 = new Web3(provider);

// finished preparation
```

## Deployment to Testnet

```js
// deploy.js
const HDWalletProvider = require('@truffle/hdwallet-provider');
const { Web3 } = require('web3');
const { interface, bytecode } = require('./compile');

const provider = new HDWalletProvider(
        'REPLACE_WITH_YOUR_MNEMONIC',
        // remember to change this to your own phrase!
        'REPLACE_WITH_YOUR_INFURA_URL'
        // remember to change this to your own endpoint!
);
const web3 = new Web3(provider);

const deploy = async () => {
  const accounts = await web3.eth.getAccounts();

  console.log('Attempting to deploy from account', accounts[0]);

  const result = await new web3.eth.Contract(JSON.parse(interface))
          .deploy({ data: bytecode, arguments: ['Hi there!'] })
          .send({ gas: '1000000', from: accounts[0] });

  console.log('Contract deployed to', result.options.address);
  provider.engine.stop();
};
deploy();
```

## Observing Deployment on Etherscan

https://sepolia.etherscan.io/tx/0x6514ec1bd5877ce0f566c6b3c60cce2ce12d1ce1aa3898421c3b26c869f448b6

## Deployed Contracts in Remix

Environment: Injected Provider - Metamask

https://sepolia.etherscan.io/tx/0x5e1755af6caa5bd25613c73f91eb6dd258be0098f166ecc2b8131d745bd18627

## Project Review

## Updating Your Inbox Project to Solc v0.8.9