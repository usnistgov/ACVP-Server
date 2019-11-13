using System;
using System.Numerics;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KTS;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.KTS
{
    public class Mgf : IMgf
    {
        private ISha _sha;

        public Mgf(ISha sha)
        {
            _sha = sha;
        }
        
        public BitString Generate(BitString mgfSeed, int maskLenBits)
        {
            if (maskLenBits % BitString.BITSINBYTE != 0)
            {
                throw new ArgumentException($"{nameof(maskLenBits)} should be mod {BitString.BITSINBYTE}");
            }

            var maskLen = maskLenBits.CeilingDivide(BitString.BITSINBYTE);
            var hashLen = _sha.HashFunction.OutputLen.CeilingDivide(BitString.BITSINBYTE);
            
            // 1. If maskLen > 2^32*hashLen, output an error indicator, and exit from this process without
            // performing the remaining actions.
            
            // 2. If mgfSeed is more than max_hash_inputLen bytes in length, then output an error indicator,
            // and exit this process without performing the remaining actions.
            if (_sha.HashFunction.MaxMessageLen != -1 && mgfSeed.BitLength > _sha.HashFunction.MaxMessageLen)
            {
                throw new ArgumentException($"{nameof(mgfSeed)} length must not exceed the hash functions max message size.");
            }

            // 3. Set T = the null string.
            var T = new BitString(0);
            
            // 4. For counter from 0 to  maskLen / hashLen  – 1, do the following:
            var loopMax = maskLen.CeilingDivide(hashLen) - 1;
            for (var counter = 0; counter <= loopMax; counter++)
            {
                // a) Let D = I2BS(counter, 4) (see Appendix B.1).
                var D = BitString.To32BitString(counter);

                // b) Let T = T || hash(mgfSeed || D).
                T = T.ConcatenateBits(_sha.HashMessage(mgfSeed.ConcatenateBits(D)).Digest);
            }

            return T.GetMostSignificantBits(maskLenBits);
        }
    }
}