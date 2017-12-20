using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.CTR.Enums;
using NIST.CVP.Crypto.CTR.Helpers;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.CTR
{
    public class TestableCounter : ICounter
    {
        private readonly List<BitString> _ivs;
        private int _currentIndex = 0;
        private readonly int _blockSize;

        public TestableCounter(Cipher cipher, List<BitString> ivs)
        {
            _ivs = new List<BitString>();
            _blockSize = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(cipher).blockSize;

            foreach (var iv in ivs)
            {
                // If the IV is too short to actually be an IV, pad some 0s to the MSB side.
                if (iv.BitLength < _blockSize)
                {
                    _ivs.Add(BitString.Zeroes(_blockSize - iv.BitLength).ConcatenateBits(iv));
                }
                else
                {
                    _ivs.Add(iv);
                }
            }
        }

        public BitString GetNextIV()
        {
            if (_currentIndex >= _ivs.Count)
            {
                throw new Exception("Cannot get another iv for this counter");
            }

            var currentIV = _ivs[_currentIndex];
            _currentIndex++;

            return currentIV.GetLeastSignificantBits(_blockSize);
        }
    }
}
