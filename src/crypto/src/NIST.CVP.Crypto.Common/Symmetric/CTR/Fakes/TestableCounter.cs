using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.CTR.Fakes
{
    public class TestableCounter : ICounter
    {
        private readonly List<BitString> _ivs;
        private int _currentIndex = 0;
        private readonly int _blockSize;

        public TestableCounter(IBlockCipherEngine engine, List<BitString> ivs)
        {
            _ivs = new List<BitString>();
            _blockSize = engine.BlockSizeBits;

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
