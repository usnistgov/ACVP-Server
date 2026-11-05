using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.Blake2;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Blake2
{
    public class Blake2Factory : IBlake2Factory
    {
        public IBlake2 GetBlake2Instance(Blake2HashFunction hashFunction)
        {
            return hashFunction.Variant switch
            {
                Blake2Variant.Blake2b => new Blake2b(hashFunction),
                _ => throw new ArgumentException("BLAKE2s is not implemented yet.", nameof(hashFunction))
            };
        }
    }
}
