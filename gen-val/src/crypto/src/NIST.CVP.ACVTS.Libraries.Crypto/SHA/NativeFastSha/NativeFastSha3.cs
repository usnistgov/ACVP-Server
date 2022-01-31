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
    public class NativeFastSha3 : NativeFastKeccakBase, ISha
    {
        public HashFunction HashFunction
        {
            get
            {
                var hashFunction = ShaAttributes.GetShaAttributes()
                    .FirstOrDefault(sha => sha.mode == ModeValues.SHA3 && sha.outputLen == _bitLength);

                return new HashFunction(hashFunction.mode, hashFunction.digestSize);
            }
        }

        private readonly int _bitLength;
        private BitString _cachedBits;
        private readonly BitString _endBits = new BitString("02", 2, false);

        public NativeFastSha3(int bitLength)
        {
            _bitLength = CheckBitLength(bitLength);
        }

        public HashResult HashMessage(BitString message, int outLen = 0)
        {
            var digest = new byte[_bitLength / 8];

            Init();
            Update(message.GetPaddedBytes(), message.BitLength);
            Final(digest);

            return new HashResult(new BitString(digest));
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
            var digest = new byte[_bitLength / 8];
            var iterations = message.FullLength / message.ContentLength;
            var paddedBytes = message.Content.GetPaddedBytes();

            Init();

            for (var i = 0; i < iterations; i++)
            {
                Update(paddedBytes, message.ContentLength);
            }

            Final(digest);

            return new HashResult(new BitString(digest));
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
                FinalBits(output, 0, _cachedBits.ToBytes().First(), _cachedBits.BitLength);
            }
            else
            {
                AbsorbBits(_endBits.ToBytes().First(), 2);
                base.Final(output, 0);
            }
        }

        private void FinalBits(byte[] output, int outOff, byte partialByte, int partialBits)
        {
            if (partialBits < 0 || partialBits > 7)
            {
                throw new ArgumentException("must be in the range [0,7]", nameof(partialBits));
            }

            var finalInput = (partialByte & ((1 << partialBits) - 1)) | (0x02 << partialBits);
            var finalBits = partialBits + 2;

            if (finalBits >= 8)
            {
                Absorb((byte)finalInput);
                finalBits -= 8;
                finalInput >>= 8;
            }

            DoFinal(output, outOff, (byte)finalInput, finalBits);
        }


        private static int CheckBitLength(int bitLength)
        {
            switch (bitLength)
            {
                case 224:
                case 256:
                case 384:
                case 512:
                    return bitLength;
                default:
                    throw new ArgumentException(bitLength + " not supported for SHA-3", nameof(bitLength));
            }
        }
    }
}
