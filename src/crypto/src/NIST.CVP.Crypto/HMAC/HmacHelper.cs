using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.HMAC
{
    public static class HmacHelper
    {
        public static BitString PadKey(BitString key, ISha sha)
        {
            var keyLength = key.BitLength;
            var blockSize = sha.HashFunction.BlockSize;
            
            // Step 1 If the length of K = B: set K0 = K.Go to step 4.
            if (keyLength == blockSize)
            {
                return key.GetDeepCopy();
            }
            // Step 2 If the length of K > B: hash K to obtain an L byte string, then append(B - L) zeros to create a B - byte string K0(i.e., K0 = H(K) || 00...00). Go to step 4.
            else if (keyLength > blockSize)
            {
                var hashedKey = sha.HashMessage(key).Digest;
                return hashedKey.ConcatenateBits(BitString.Zeroes(blockSize - hashedKey.BitLength)).GetDeepCopy();
            }
            // Step 3 If the length of K < B: append zeros to the end of K to create a B - byte string K0 (e.g., if K is 20 bytes in length and B = 64, then K will be appended with 44 zero bytes x’00’).
            else
            {
                return key.ConcatenateBits(BitString.Zeroes(blockSize - keyLength)).GetDeepCopy();
            }
        }

        public static BitString GetBitStringWithRepeatedBytes(byte byteToConcatenate, int hashFunctionBlockSize)
        {
            var bitString = new BitString(0);

            while (bitString.BitLength < hashFunctionBlockSize)
            {
                bitString = bitString.ConcatenateBits(new BitString(new byte[] {byteToConcatenate}));
            }

            return bitString;
        }
    }
}