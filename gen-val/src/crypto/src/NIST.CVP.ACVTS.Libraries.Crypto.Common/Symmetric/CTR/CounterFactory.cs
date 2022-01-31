using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.CTR
{
    public class CounterFactory : ICounterFactory
    {
        public ICounter GetCounter(IBlockCipherEngine engine, CounterTypes counterType, BitString initialIv)
        {
            switch (counterType)
            {
                case CounterTypes.Additive:
                    return new AdditiveCounter(engine, initialIv);
                case CounterTypes.Subtractive:
                    return new SubtractiveCounter(engine, initialIv);
                default:
                    throw new ArgumentException(nameof(counterType));
            }
        }
    }
}
