using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.HMAC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.HMAC
{
    public class Hmac : IHmac
    {
        private readonly ISha _iSha;
        private readonly byte _iPad = 0x36;
        private readonly byte _oPad = 0x5c;

        public int OutputLength => _iSha.HashFunction.OutputLen;

        public Hmac(ISha iSha)
        {
            _iSha = iSha;
        }
        
        public MacResult Generate(BitString key, BitString message, int macLength = 0)
        {
            int blockSize = _iSha.HashFunction.BlockSize;

            if (macLength > blockSize)
            {
                return new MacResult($"Requested {nameof(macLength)} of {macLength} is greater than the {nameof(blockSize)} of {blockSize}");
            }
            
            // Step 1-3
            var paddedKey_K0 = PadKey(key);

            // Step 4 Exclusive - Or K0 with ipad to produce a B - byte string: K0 ⊕ ipad.
            var xoredK0WithIpad = paddedKey_K0.XOR(GetBitStringWithRepeatedBytes(_iPad, blockSize));
            
            // Step 5 Append the stream of data 'text' to the string resulting from step 4: (K0 ⊕ ipad) || text.
            var preHashedStream = xoredK0WithIpad.ConcatenateBits(message);
            
            // Step 6 Apply H to the stream generated in step 5: H((K0 ⊕ ipad) || text).
            var hashedStream = _iSha.HashMessage(preHashedStream).Digest;
            
            // Step 7 Exclusive - Or K0 with opad: K0 ⊕ opad.
            var xoredK0WithOpad = paddedKey_K0.XOR(GetBitStringWithRepeatedBytes(_oPad, blockSize));
            
            // Step 8 Append the result from step 6 to step 7: (K0 ⊕ opad) || H((K0 ⊕ ipad) || text).
            var concatXoredK0OpadWithHashedStream = xoredK0WithOpad.ConcatenateBits(hashedStream);
            
            // Step 9 Apply H to the result from step 8: H((K0 ⊕ opad) || H((K0 ⊕ ipad) || text)).
            var final = _iSha.HashMessage(concatXoredK0OpadWithHashedStream).Digest;

            if (macLength == 0)
            {
                macLength = final.BitLength;
            }
            
            return new MacResult(final.GetMostSignificantBits(macLength));
        }

        private BitString PadKey(BitString key)
        {
            var keyLength = key.BitLength;
            var blockSize = _iSha.HashFunction.BlockSize;
            
            // Step 1 If the length of K = B: set K0 = K.Go to step 4.
            if (keyLength == blockSize)
            {
                return key.GetDeepCopy();
            }
            // Step 2 If the length of K > B: hash K to obtain an L byte string, then append(B - L) zeros to create a B - byte string K0(i.e., K0 = H(K) || 00...00). Go to step 4.
            else if (keyLength > blockSize)
            {
                var hashedKey = _iSha.HashMessage(key).Digest;
                return hashedKey.ConcatenateBits(BitString.Zeroes(blockSize - hashedKey.BitLength)).GetDeepCopy();
            }
            // Step 3 If the length of K < B: append zeros to the end of K to create a B - byte string K0 (e.g., if K is 20 bytes in length and B = 64, then K will be appended with 44 zero bytes x’00’).
            else
            {
                return key.ConcatenateBits(BitString.Zeroes(blockSize - keyLength)).GetDeepCopy();
            }
        }

        private BitString GetBitStringWithRepeatedBytes(byte byteToConcatenate, int hashFunctionBlockSize)
        {
            BitString bitString = new BitString(0);

            while (bitString.BitLength < hashFunctionBlockSize)
            {
                bitString = bitString.ConcatenateBits(new BitString(new byte[] {byteToConcatenate}));
            }

            return bitString;
        }
    }
}
