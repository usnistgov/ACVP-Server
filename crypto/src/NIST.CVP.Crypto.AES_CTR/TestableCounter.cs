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
            _ivs = ivs;
        }

        public BitString GetNextIV()
        {
            if (_currentIndex >= _ivs.Count)
            {
                throw new Exception("Cannot get another iv for this counter");
            }

            var currentIV = _ivs[_currentIndex];
            _currentIndex++;

            return currentIV;
        }
    }
}
