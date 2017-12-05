using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.CTR.Enums;
using NIST.CVP.Crypto.CTR.Helpers;
using NIST.CVP.Generation.Core.Helpers;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.CTR
{
    /// <summary>
    /// A simple <see cref="ICounter"/> with wrapping. Starts at 000...0 by default and increases by 1 each request
    /// </summary>
    public class AdditiveCounter : ICounter
    {
        private BitString _iv;
        private readonly int _blockSize;

        public AdditiveCounter(Cipher cipher, BitString initialIV)
        {
            _blockSize = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(cipher).blockSize;
            _iv = BitString.Zeroes(_blockSize);

            // If the IV is too short to actually be an IV, pad some 0s to the MSB side.
            if (initialIV.BitLength < _blockSize)
            {
                initialIV = BitString.Zeroes(_blockSize - initialIV.BitLength).ConcatenateBits(initialIV);
            }

            _iv = initialIV;
        }

        public BitString GetNextIV()
        {
            var currentIV = _iv.GetLeastSignificantBits(_blockSize).GetDeepCopy();
            _iv = _iv.BitStringAddition(BitString.One());

            return currentIV;
        }
    }
}
