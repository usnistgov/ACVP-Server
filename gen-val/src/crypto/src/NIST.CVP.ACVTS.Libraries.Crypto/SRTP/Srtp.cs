using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SRTP;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SRTP
{
    public class Srtp : ISrtp
    {
        private readonly IModeBlockCipher<SymmetricCipherResult> _aesEcb;

        public Srtp()
        {
            _aesEcb = new EcbBlockCipher(new AesEngine());
        }

        public KdfResult DeriveKey(int keyLength, BitString keyMaster, BitString saltMaster, BitString kdr, BitString index, BitString srtcpIndex)
        {
            if (keyMaster.BitLength != 128 && keyMaster.BitLength != 192 && keyMaster.BitLength != 256)
            {
                return new KdfResult("Invalid Master Key, must be 128, 192, or 256-bit");
            }

            if (saltMaster.BitLength != 112)
            {
                return new KdfResult("Invalid Master Salt, must be 112-bit");
            }

            if (index.BitLength != 48)
            {
                return new KdfResult("Invalid SRTP Index, must be 48-bit");
            }

            if (srtcpIndex.BitLength != 32)
            {
                return new KdfResult("Invalid SRTCP Index, must be 32-bit");
            }

            // Ke = Encryption Key, Ka = Authentication Key (for a HMAC-SHA-1), Ks = Salting Key
            var srtpKe = Kdf(keyLength, keyMaster, saltMaster, kdr, index, new BitString("00"));
            var srtpKa = Kdf(160, keyMaster, saltMaster, kdr, index, new BitString("01"));
            var srtpKs = Kdf(112, keyMaster, saltMaster, kdr, index, new BitString("02"));
            var srtpResult = new SrtpResult(srtpKe, srtpKa, srtpKs);

            var srtcpKe = Kdf(keyLength, keyMaster, saltMaster, kdr, srtcpIndex, new BitString("03"));
            var srtcpKa = Kdf(160, keyMaster, saltMaster, kdr, srtcpIndex, new BitString("04"));
            var srtcpKs = Kdf(112, keyMaster, saltMaster, kdr, srtcpIndex, new BitString("05"));
            var srtcpResult = new SrtpResult(srtcpKe, srtcpKa, srtcpKs);

            return new KdfResult(srtpResult, srtcpResult);
        }

        private BitString Kdf(int length, BitString key, BitString salt, BitString kdr, BitString index, BitString label)
        {
            var keyId = label.ConcatenateBits(Divide(index, kdr));
            var m = length.CeilingDivide(128); // guaranteed to be either 1 or 2

            // Left pad keyId with 0s so it is the same length as salt
            keyId = BitString.Zeroes(salt.BitLength - keyId.BitLength).ConcatenateBits(keyId);
            var x = keyId.XOR(salt);

            var derivedKey = new BitString(0);
            for (short i = 0; i < m; i++)
            {
                var inBlock = x.ConcatenateBits(BitString.To16BitString(i));
                var param = new ModeBlockCipherParameters(BlockCipherDirections.Encrypt, key, inBlock);
                derivedKey = derivedKey.ConcatenateBits(_aesEcb.ProcessPayload(param).Result);
            }

            return derivedKey.GetMostSignificantBits(length);
        }

        private BitString Divide(BitString a, BitString b)
        {
            if (b.Equals(BitString.Zeroes(b.BitLength)))
            {
                return BitString.Zeroes(a.BitLength);
            }

            var aBigInt = a.ToPositiveBigInteger();
            var bBigInt = b.ToPositiveBigInteger();

            var result = new BitString(aBigInt / bBigInt);

            // Ensure result is the same number of bits as a
            if (result.BitLength > a.BitLength)
            {
                result = result.GetLeastSignificantBits(a.BitLength);
            }
            else if (result.BitLength < a.BitLength)
            {
                result = BitString.Zeroes(a.BitLength - result.BitLength).ConcatenateBits(result);
            }

            return result;
        }
    }
}
