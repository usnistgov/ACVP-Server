using NIST.CVP.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric
{
    public interface ICounterFactory
    {
        ICounter GetCounter(IBlockCipherEngine engine, CounterTypes counterType, BitString initialIv);
    }
}