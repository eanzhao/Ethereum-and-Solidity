using System.Diagnostics;
using System.Text.Json.Nodes;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Shouldly;

namespace EAS.Section2.Tests;

public class NethereumTests
{
    private readonly Web3 _web3 = new(new Account(TestData.PrivateKey), TestData.Url);
    private const string SolFilePath = "contracts/Inbox.sol";

    [Fact]
    public async Task<string[]> ListAccountsTest()
    {
        var accounts = await _web3.Eth.Accounts.SendRequestAsync();
        accounts.Length.ShouldBePositive();
        return accounts;
    }

    [Fact]
    public async Task<string> DeployContractTest()
    {
        //var accounts = await ListAccountsTest();
        var address = TestData.Address;
        var (abi, bytecode) = await GetAbiAndBinAsync(SolFilePath);

        var gas = await _web3.Eth.DeployContract.EstimateGasAsync(abi, bytecode, address, "Hi There");
        var transactionReceipt = await _web3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(
            abi,
            bytecode,
            address,
            gas,
            new HexBigInteger(0),
            null,
            "Hi There");
        var contractAddress = transactionReceipt.ContractAddress;
        contractAddress.ShouldNotBeNull();
        return contractAddress;
    }

    [Fact]
    public async Task GetMessageTest()
    {
        var contractAddress = await DeployContractTest();
        var (abi, _) = await GetAbiAndBinAsync(SolFilePath);
        var contract = _web3.Eth.GetContract(abi, contractAddress);
        var getMessage = contract.GetFunction("getMessage");
        var message = await getMessage.CallAsync<string>();
        message.ShouldBe("Hi There");
    }

    private async Task<(string, string)> GetAbiAndBinAsync(string solidityFilePath)
    {
        var output = await CompileSolidityFileAsync(solidityFilePath);
        var json = JsonNode.Parse(output)!["contracts"]!.AsObject();
        var combined = new JsonObject();
        foreach (var foo in json)
        {
            combined = foo.Value!.AsObject();
        }

        var abi = combined["abi"]!.ToJsonString();
        var bin = combined["bin"]!.ToString();
        return (abi, bin);
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