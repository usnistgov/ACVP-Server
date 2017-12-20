using NIST.CVP.Crypto.CTR.Enums;
using NIST.CVP.Crypto.CTR.Helpers;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.CTR
{
    /// <summary>
    /// A simple <see cref="ICounter"/> with wrapping. Starts at FFF...F by default and decreases by 1 each request
    /// </summary>
    public class SubtractiveCounter : ICounter
    {
        private BitString _iv;
        private readonly int _blockSize;

        public SubtractiveCounter(Cipher cipher, BitString initialIV)
        {
            _blockSize = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(cipher).blockSize;
            _iv = BitString.Ones(_blockSize);

            // If the IV is too short to actually be an IV, pad some 0s to the MSB side.
            if (initialIV.BitLength < _blockSize)
            {
                initialIV = BitString.Ones(_blockSize - initialIV.BitLength).ConcatenateBits(initialIV);
            }

            _iv = initialIV;
        }

        public BitString GetNextIV()
        {
            if (_iv.BitLength < _blockSize)
            {
                _iv = BitString.Zeroes(_blockSize - _iv.BitLength).ConcatenateBits(_iv);
            }
            
            var currentIV = _iv.GetLeastSignificantBits(_blockSize).GetDeepCopy();
    
            // Avoid throwing an exception by subtracting from 000...0
            if (currentIV.Equals(BitString.Zeroes(_blockSize)))
            {
                _iv = BitString.Ones(_blockSize);
            }
            else
            {
                _iv = _iv.BitStringSubtraction(BitString.One());
            }

            return currentIV;
        }
    }
}
