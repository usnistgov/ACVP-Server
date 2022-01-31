using System;
using System.Linq;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.LargeBitString;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha
{
    public class NativeFastShake : NativeFastKeccakBase, ISha
    {
        public HashFunction HashFunction
        {
            get
            {
                var hashFunction = ShaAttributes.GetShaAttributes()
                    .FirstOrDefault(sha => sha.mode == ModeValues.SHAKE && sha.outputLen == _bitLength);

                return new HashFunction(hashFunction.mode, hashFunction.digestSize);
            }
        }

        private readonly int _bitLength;
        private BitString _cachedBits;
        private readonly BitString _endBits = new BitString("0F", 4, false);

        public NativeFastShake(int bitLength)
        {
            _bitLength = CheckBitLength(bitLength);
        }

        public HashResult HashMessage(BitString message, int outLen = 0)
        {
            if (outLen == 0)
            {
                outLen = _bitLength;
            }

            // If outputBitLength is not a multiple of 8, make it one
            var requestedBitLength = outLen;
            if (outLen % 8 != 0)
            {
                outLen += 8 - (outLen % 8);
            }

            var digest = new byte[outLen / 8];

            Init();
            Update(message.GetPaddedBytes(), message.BitLength);
            Final(digest, outLen);

            return requestedBitLength == outLen ? new HashResult(new BitString(digest)) : new HashResult(LittleEndianSubstring(new BitString(digest), 0, requestedBitLength));
        }

        public HashResult HashNumber(BigInteger number)
        {
            var bs = new BitString(number);

            // Pad the BitString to be a multiple of 32 bits
            // Likely a relic of old MultiPrecision libraries
            // Spec says to just hash the integer which is normally 4 bytes but 
            //      with larger integer values, libraries keep them at multiples
            //      of 4 bytes, so we have to make sure our structure is a multiple
            //      of 4 bytes as well.
            if (bs.BitLength % 32 != 0)
            {
                bs = BitString.ConcatenateBits(BitString.Zeroes(32 - bs.BitLength % 32), bs);
            }

            return HashMessage(bs);
        }

        public HashResult HashLargeMessage(LargeBitString message)
        {
            throw new NotSupportedException();
        }

        public void Init()
        {
            Init(_bitLength);
            _cachedBits = new BitString(0);
        }

        public void Update(byte[] input, int bitLength)
        {
            if (bitLength % 8 == 0 && _cachedBits.BitLength == 0)
            {
                BlockUpdate(input, 0, bitLength / 8);
                return;
            }

            var inputBits = new BitString(input).GetMostSignificantBits(bitLength);
            _cachedBits = BitString.ConcatenateBits(_cachedBits, inputBits);
            var numberOfCompletedBytes = _cachedBits.BitLength / 8;

            if (numberOfCompletedBytes > 0)
            {
                var completedBytes = _cachedBits.GetMostSignificantBits(numberOfCompletedBytes * 8);
                BlockUpdate(completedBytes.ToBytes(), 0, completedBytes.BitLength / 8);
            }

            _cachedBits = numberOfCompletedBytes * 8 == _cachedBits.BitLength ? new BitString(0) : _cachedBits.GetLeastSignificantBits(_cachedBits.BitLength - numberOfCompletedBytes * 8);
        }

        public new void Final(byte[] output, int outputBitLength = 0)
        {
            if (_cachedBits.BitLength != 0)
            {
                FinalBits(output, 0, outputBitLength, _cachedBits.ToBytes().First(), _cachedBits.BitLength);
            }
            else
            {
                AbsorbBits(_endBits.ToBytes().First(), 4);
                base.Final(output, 0, outputBitLength);
            }
        }

        private void FinalBits(byte[] output, int outOff, int outputBitLength, byte partialByte, int partialBits)
        {
            if (partialBits < 0 || partialBits > 7)
            {
                throw new ArgumentException("must be in the range [0,7]", nameof(partialBits));
            }

            var finalInput = (partialByte & ((1 << partialBits) - 1)) | (0x0F << partialBits);
            var finalBits = partialBits + 4;

            if (finalBits >= 8)
            {
                Absorb((byte)finalInput);
                finalBits -= 8;
                finalInput >>= 8;
            }

            if (finalBits > 0)
            {
                AbsorbBits(finalInput, finalBits);
            }

            Squeeze(output, outOff, outputBitLength);
        }

        private static int CheckBitLength(int bitLength)
        {
            switch (bitLength)
            {
                case 128:
                case 256:
                    return bitLength;
                default:
                    throw new ArgumentException(bitLength + " not supported for SHAKE", nameof(bitLength));
            }
        }

        // Use this method when you need a Little Endian substring that is not a multiple of 8 bits in length
        // For the last byte, we would need to pull bits from the other end of the byte, rather than like reading an array in order
        // This only occurs once under SHAKE with variable output sizes
        private static BitString LittleEndianSubstring(BitString message, int startIdx, int length)
        {
            var lastFullByte = (length / 8) * 8;    // Integer division rounds down for us
            var firstBytes = BitString.MSBSubstring(message, startIdx, lastFullByte);

            if (length == lastFullByte)
            {
                return firstBytes;
            }

            var nextByte = BitString.MSBSubstring(message, startIdx + lastFullByte, 8);

            var bitsNeeded = length % 8;
            var lastBits = new BitString(0);
            if (bitsNeeded != 0)
            {
                lastBits = BitString.Substring(nextByte, 0, bitsNeeded);
            }

            return BitString.ConcatenateBits(firstBytes, lastBits);
        }
    }
}
