using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;
using System.Numerics;

namespace NIST.CVP.Crypto.RSA.Signatures
{
    public class RSASSA_PSS_Signer : SignerBase
    {
        public RSASSA_PSS_Signer(HashFunction hashFunction, int saltLen) : base(hashFunction, saltLen) { }

        // B.2.1 Mask Generation Function 1 from RFC-3447
        private BitString MGF(BitString seed, int maskLen)
        {
            var T = new BitString(0);
            var hLen = SHAEnumHelpers.DigestSizeToInt(_hashFunction.DigestSize);
            var iterations = (int)NumberTheory.CeilingDivide(maskLen, hLen) - 1;

            for(var i = 0; i < iterations; i++)
            {
                var counter = (BigInteger)i;
                var C = new BitString(counter, 32);
                var dig = Hash(BitString.ConcatenateBits(seed, C));

                T = BitString.ConcatenateBits(T, dig);
            }

            return T.MSBSubstring(0, maskLen);
        }

        // 9.1.1 Encoding Operation from RFC-3447
        private BitString EMSA_PSS_Encode(BitString message, int emBits)
        {
            var mHash = Hash(message);

            var emLen = (int)NumberTheory.CeilingDivide(emBits, 8);
            if(emLen < mHash.BitLength + _saltLen * 8 + 2 * 8)
            {
                throw new Exception("Encoding error, RSA-SigGen-PSS");
            }

            var salt = _rand.GetRandomBitString(_saltLen * 8);
            var mPrime = BitString.Zeroes(64);
            mPrime = BitString.ConcatenateBits(mPrime, mHash);
            mPrime = BitString.ConcatenateBits(mPrime, salt);

            var H = Hash(mPrime);
            var PS = BitString.Zeroes(emLen * 8 - _saltLen * 8 - H.BitLength - 2 * 8);

            var DB = PS.GetDeepCopy();
            DB = BitString.ConcatenateBits(DB, new BitString("01"));
            DB = BitString.ConcatenateBits(DB, salt);

            var dbMask = MGF(H, emLen * 8 - H.BitLength - 1 * 8);
            var maskedDB = BitString.XOR(DB, dbMask);

            // Set leftmost bits to 0
            for(var i = 0; i < 8 * emLen - emBits; i++)
            {
                maskedDB.Set(maskedDB.BitLength - i - 1, false);
            }

            var EM = maskedDB.GetDeepCopy();
            EM = BitString.ConcatenateBits(EM, H);
            EM = BitString.ConcatenateBits(EM, new BitString("BC"));

            return EM;
        }

        public override SignatureResult Sign(int nlen, BitString message, KeyPair key)
        {
            // 1. EMSA_PSS_Encode
            var EM = EMSA_PSS_Encode(message, nlen - 1);

            var signature = Decrypt(key.PubKey.N, key.PrivKey.D, EM.ToPositiveBigInteger());
            return new SignatureResult(new BitString(signature));
        }

        public override VerifyResult Verify(int nlen, BitString signature, KeyPair key, BitString message)
        {
            throw new NotImplementedException();
        }
    }
}
