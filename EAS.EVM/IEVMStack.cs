using System.Numerics;

namespace EAS.EVM;

public interface IEVMStack
{
    BigInteger Pop();
    BigInteger Push(object obj);
}