using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_CTR
{
    public class TestableCounter : ICounter
    {
        private readonly List<BitString> _ivs;
        private int _currentIndex = 0;

        public TestableCounter(List<BitString> ivs)
        {
            _ivs = new List<BitString>();

            foreach (var iv in ivs)
            {
                // If the IV is too short to actually be an IV, pad some 0s to the MSB side.
                if (iv.BitLength < 128)
                {
                    _ivs.Add(BitString.Zeroes(128 - iv.BitLength).ConcatenateBits(iv));
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

            return currentIV.GetLeastSignificantBits(128);
        }
    }
}
