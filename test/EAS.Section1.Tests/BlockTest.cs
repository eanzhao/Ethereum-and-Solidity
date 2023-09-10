using Shouldly;
using Xunit.Abstractions;

namespace EAS.Section1.Tests;

public class BlockTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public BlockTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void MineTest()
    {
        var severalZeros = Constants.Difficulty.ToAllZeroStringOfSameCount();
        var blockWithHash = new BlockWithHash
        {
            Block = new Block(1, 1, "hello world", "0000000000000000000000000000000000000000000000000000000000000000")
        };
        while (!blockWithHash.Hash.StartsWith(severalZeros))
        {
            blockWithHash.IncreaseNonce();
        }

        blockWithHash.Hash[..4].ShouldBe(severalZeros);
        blockWithHash.Block.Nonce.ShouldBe(85460L);
        blockWithHash.Print();
    }
}