using System;
using System.Collections.Generic;

namespace NIST.CVP.Math.Entropy
{
    public class TestableEntropyProvider : IEntropyProvider, ITestableEntropyProvider
    {
        private readonly List<BitString> _entropy = new List<BitString>();

        public void AddEntropy(BitString entropy)
        {
            _entropy.Add(entropy);
        }

        public BitString GetEntropy(int numberOfBits)
        {
            if (_entropy.Count == 0)
            {
                throw new Exception($"No Entropy exists within provider");
            }

            if (numberOfBits != _entropy[0].BitLength)
            {
                throw new ArgumentException($"expected {nameof(numberOfBits)} does not match the number of bits in {nameof(_entropy)}");
            }

            var entropy = _entropy[0];
            _entropy.RemoveAt(0);

            return entropy;
        }
    }
}
