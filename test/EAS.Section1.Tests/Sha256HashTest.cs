using System.Security.Cryptography;
using Shouldly;

namespace EAS.Section1.Tests;

public class Sha256HashTest
{
    [Fact]
    public void Sha256Hash()
    {
        var data1 = "foo"u8.ToArray();
        var data2 = "bar"u8.ToArray();
        SHA256.HashData(data1).ShouldNotBe(SHA256.HashData(data2));

        Convert.ToHexString(SHA256.HashData("anders"u8.ToArray()))
            .ShouldBe("19EA4AC2E1A53B1267FE5A61A3B6B81F760CE4223A25B495A5E2B6183DA68717");

        Convert.ToHexString(SHA256.HashData(""u8.ToArray()))
            .ShouldBe("E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855");
    }
}