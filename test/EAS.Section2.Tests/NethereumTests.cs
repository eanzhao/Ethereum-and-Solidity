using System.Diagnostics;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Newtonsoft.Json;
using Shouldly;

namespace EAS.Section2.Tests;

public class NethereumTests
{
    [Fact]
    public async Task<string[]> ListAccountsTest()
    {
        var web3 = new Web3("http://localhost:7545");
        var accounts = await web3.Eth.Accounts.SendRequestAsync();
        accounts.Length.ShouldBePositive();
        return accounts;
    }

    [Fact]
    public async Task DeployContractTest()
    {
        var accounts = await ListAccountsTest();
        var output = await CompileSolidityFileAsync("contracts/Inbox.sol");
        
        var web3 = new Web3("http://localhost:7545");

        var deploymentMessage = new InboxDeployment
        {
            InitialMessage = "Hi There"
        };
        
        var deploymentHandler = web3.Eth.GetContractDeploymentHandler<InboxDeployment>();
        var transactionReceipt = await deploymentHandler.SendRequestAndWaitForReceiptAsync(deploymentMessage);
        var contractAddress = transactionReceipt.ContractAddress;
    }

    private async Task<string> CompileSolidityFileAsync(string solidityFilePath)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "solc",
                Arguments = $"--combined-json abi,bin {solidityFilePath}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };
        process.Start();
        return await process.StandardOutput.ReadToEndAsync();
    }
}