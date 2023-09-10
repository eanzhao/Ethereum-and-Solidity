namespace EAS.Section1;

public static class Extensions
{
    public static void IncreaseNonce(this BlockWithHash blockWithHash)
    {
        var block = blockWithHash.Block;
        blockWithHash.Block = block with { Nonce = block.Nonce + 1 };
    }

    public static string ToAllZeroStringOfSameCount(this int count)
    {
        var str = string.Empty;
        for (var i = 0; i < count; i++)
        {
            str += '0';
        }

        return str;
    }

    public static bool CheckDifficulty(this string hash, int difficulty)
    {
        return hash.StartsWith(difficulty.ToAllZeroStringOfSameCount());
    }

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
}