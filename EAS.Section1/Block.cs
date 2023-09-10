using System.Security.Cryptography;
using System.Text;
using Spectre.Console;

namespace EAS.Section1;

public record Block(long Number, long Nonce, string Data, string PreviousBlockHash);

public class BlockWithHash
{
    public required Block Block { get; set; }

    public string Hash =>
        Convert.ToHexString(
            SHA256.HashData(
                Encoding.UTF8.GetBytes($"{Block.Number}{Block.Nonce}{Block.Data}{Block.PreviousBlockHash}")));

    public void Print()
    {
        var table = new Table();
        table.AddColumn("Block:");
        table.AddColumn("#" + Block.Number);
        table.AddRow("Nonce:", $"{Block.Nonce}");
        table.AddRow("Data:", $"{Block.Data}");
        table.AddRow("Prev:", $"{Block.PreviousBlockHash}");
        table.AddRow("Hash:", $"{Hash}");

        AnsiConsole.Write(table);
    }
}