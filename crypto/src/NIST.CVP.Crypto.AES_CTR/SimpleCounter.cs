using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_CTR
{
    /// <summary>
    /// A simple <see cref="ICounter"/> with wrapping. Starts at 000...0 by default and increases by 1 each request
    /// </summary>
    public class SimpleCounter : ICounter
    {
        private BitString _iv = BitString.Zeroes(128);

        public SimpleCounter() { }

        public SimpleCounter(BitString initialIV)
        {
            // If the IV is too short to actually be an IV, pad some 0s to the MSB side.
            if (initialIV.BitLength < 128)
            {
                initialIV = BitString.Zeroes(128 - initialIV.BitLength).ConcatenateBits(initialIV);
            }

            _iv = initialIV;
        }

        public BitString GetNextIV()
        {
            var currentIV = _iv.GetLeastSignificantBits(128).GetDeepCopy();
            _iv = _iv.BitStringAddition(BitString.One());

            return currentIV;
        }
    }
}
