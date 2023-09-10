namespace EAS.Section1;

public class Blockchain
{
    public List<BlockWithHash> BlockList { get; set; } = new();

    public bool Validate()
    {
        if (!BlockList.Any())
        {
            return false;
        }

        var previousBlockHash = BlockList.First().Hash;
        foreach (var blockWithHash in BlockList)
        {
            if (!blockWithHash.Hash.StartsWith(Constants.Difficulty.ToAllZeroStringOfSameCount()))
            {
                return false;
            }

            if (blockWithHash.Block.Number == 1)
            {
                continue;
            }

            if (blockWithHash.Block.PreviousBlockHash != previousBlockHash) return false;
            previousBlockHash = blockWithHash.Hash;
        }

        return true;
    }
}