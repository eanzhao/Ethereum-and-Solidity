//updated web3 and hdwallet-provider imports added for convenience
const HDWalletProvider = require('@truffle/hdwallet-provider');
const { Web3 } = require('web3');
const { interface, bytecode } = require('./compile');

// deploy code will go here
const provider = new HDWalletProvider(
    '',
    // remember to change this to your own phrase!
    'https://goerli.infura.io/v3/2418dfa36f574a8c828d61971d644b5b'
    // remember to change this to your own endpoint!
);
const web3 = new Web3(provider);

const deploy = async () => {
    const accounts = await web3.eth.getAccounts();

    console.log('Attempting to deploy from account', accounts[1]);

    const result = await new web3.eth.Contract(JSON.parse(interface))
        .deploy({ data: bytecode, arguments: ['Hi there!'] })
        .send({ gas: '1000000', from: accounts[1] });

    console.log('Contract deployed to', result.options.address);
    provider.engine.stop();
};

deploy();
