using Shouldly;

namespace EAS.Section1.Tests;

public class BlockchainTests
{
    [Fact]
    public void VerifyTest()
    {
        GetBlockchainForTesting().Validate().ShouldBeTrue();
    }

    [Fact]
    public void ModifyOldDataTest()
    {
        var blockchain = GetBlockchainForTesting();
        blockchain.BlockList[2].Block = blockchain.BlockList[2].Block with { Data = "Modified" };
        blockchain.Validate().ShouldBeFalse();
    }

    [Fact]
    public void ReMineTest()
    {
        var blockchain = GetBlockchainForTesting();
        blockchain.BlockList[2].Block = blockchain.BlockList[2].Block with { Data = "Modified" };
        blockchain.BlockList[2] = blockchain.BlockList[2].Mine();

        // Re-Mine block of number 4.
        blockchain.BlockList[3].Block = blockchain.BlockList[3].Block with
        {
            // Reset the PreviousBlockHash
            PreviousBlockHash = blockchain.BlockList[2].Hash
        };
        blockchain.BlockList[3] = blockchain.BlockList[3].Mine();

        // Re-Mine block of number 5.
        blockchain.BlockList[4].Block = blockchain.BlockList[4].Block with
        {
            PreviousBlockHash = blockchain.BlockList[3].Hash
        };
        blockchain.BlockList[4] = blockchain.BlockList[4].Mine();

        blockchain.Validate().ShouldBeTrue();
    }

    private Blockchain GetBlockchainForTesting()
    {
        return new Blockchain
        {
            BlockList = new List<BlockWithHash>
            {
                new()
                {
                    Block = new Block(1, 85460, "hello world",
                        "0000000000000000000000000000000000000000000000000000000000000000")
                },
                new()
                {
                    Block = new Block(2, 95864, "hello world from number 2",
                        "0000A51677B3CC64056C498C1D6C88130528CA64B9C4DE33EF694B5BF0007028")
                },
                new()
                {
                    Block = new Block(3, 86863, "hello world from number 3",
                        "0000192DEC5B22964A35C3402D136B31668F5B09BB5A399BAAD05D2E051ECEAF")
                },
                new()
                {
                    Block = new Block(4, 157426, "hello world from number 4",
                        "0000791C344914111AA1C416D7390EEAA80BC943DAEDB9556921D9DCF8A9F8B5")
                },
                new()
                {
                    Block = new Block(5, 13896, "hello world from number 5",
                        "000065ADDC8C1C67DB471465D2E50C3EE136284CDF49C3F8444219A26ECE3AE9")
                }
            }
        };
    }
}