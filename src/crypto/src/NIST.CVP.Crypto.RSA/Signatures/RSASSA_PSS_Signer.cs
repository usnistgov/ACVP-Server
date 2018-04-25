using System;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.RSA.Signatures
{
    public class RSASSA_PSS_Signer : SignerBase
    {
        public RSASSA_PSS_Signer(HashFunction hashFunction, EntropyProviderTypes entType, int saltLen) : base(hashFunction, entType, saltLen) { }
        public RSASSA_PSS_Signer(HashFunction hashFunction) : base(hashFunction) { }

        private readonly BitString _bc = new BitString("BC");
        private readonly BitString _01 = new BitString("01");

        // B.2.1 Mask Generation Function 1 from RFC-3447
        private BitString MGF(BitString seed, int maskLen)
        {
            var T = new BitString(0);
            var hLen = SHAEnumHelpers.DigestSizeToInt(_hashFunction.DigestSize);
            var iterations = maskLen.CeilingDivide(hLen) - 1;

            for (var i = 0; i <= iterations; i++)
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
            var emLen = emBits.CeilingDivide(8);

            // All byte values
            if (emLen < mHash.BitLength / 8 + _saltLen + 2)
            {
                throw new Exception("Encoding error, RSA-SigGen-PSS");
            }

            var salt = _entropy.GetEntropy(_saltLen * 8);
            var mPrime = BitString.Zeroes(64);
            mPrime = BitString.ConcatenateBits(mPrime, mHash);
            mPrime = BitString.ConcatenateBits(mPrime, salt);

            var H = Hash(mPrime);

            // All bit values
            var PS = BitString.Zeroes(emLen * 8 - _saltLen * 8 - H.BitLength - 2 * 8);

            var DB = PS.GetDeepCopy();
            DB = BitString.ConcatenateBits(DB, _01);
            DB = BitString.ConcatenateBits(DB, salt);

            // All bit values
            var dbMask = MGF(H, emLen * 8 - H.BitLength - 1 * 8);
            var maskedDB = BitString.XOR(DB, dbMask);

            // Set leftmost bits to 0
            for (var i = 0; i < 8 * emLen - emBits; i++)
            {
                maskedDB.Set(maskedDB.BitLength - i - 1, false);
            }

            var EM = maskedDB.GetDeepCopy();
            EM = BitString.ConcatenateBits(EM, H);
            EM = BitString.ConcatenateBits(EM, _bc);

            return EM;
        }

        private VerifyResult EMSA_PSS_Verify(BitString M, BitString EM, int emBits)
        {
            var mHash = Hash(M);
            if (EM.BitLength < mHash.BitLength / 8 + _saltLen + 2)
            {
                return new VerifyResult("RSA PSS Verify: inconsistent result");
            }

            if (!EM.Substring(0, 8).Equals(_bc))
            {
                return new VerifyResult("RSA PSS Verify: 'BC' hex not found");
            }

            // Bit values 
            var maskedDB = EM.MSBSubstring(0, EM.BitLength - mHash.BitLength - 8);
            var H = EM.MSBSubstring(maskedDB.BitLength, mHash.BitLength);

            if (!maskedDB.MSBSubstring(0, EM.BitLength - emBits).Equals(BitString.Zeroes(EM.BitLength - emBits)))
            {
                return new VerifyResult("RSA PSS Verify: Leading 0s not found in maskedDB");
            }

            var dbMask = MGF(H, EM.BitLength - mHash.BitLength - 8);
            var DB = BitString.XOR(maskedDB, dbMask);

            for (var i = 0; i < EM.BitLength - emBits; i++)
            {
                DB.Set(DB.BitLength - i - 1, false);
            }

            var leftmostOctets = EM.BitLength - mHash.BitLength - _saltLen * 8 - 16;
            var leftmostPosition = EM.BitLength - mHash.BitLength - _saltLen * 8 - 8;

            if(leftmostOctets != 0)
            {
                if (!DB.MSBSubstring(0, leftmostOctets).Equals(BitString.Zeroes(leftmostOctets)))
                {
                    return new VerifyResult("RSA PSS Verify: DB incorrect, improper number of leading 0s");
                }
            }

            if (!DB.MSBSubstring(leftmostOctets, 8).Equals(_01))
            {
                return new VerifyResult("RSA PSS Verify: DB incorrect, '01' byte not found");
            }

            var salt = DB.Substring(0, _saltLen * 8);
            var MPrime = BitString.Zeroes(64);
            MPrime = BitString.ConcatenateBits(MPrime, mHash);
            MPrime = BitString.ConcatenateBits(MPrime, salt);

            var HPrime = Hash(MPrime);

            if (HPrime.Equals(H))
            {
                return new VerifyResult();
            }
            else
            {
                return new VerifyResult("RSA PSS Verify: Hashes do not match");
            }
        }

        public override SignatureResult Sign(int nlen, BitString message, IKeyPair key)
        {
            // 1. EMSA_PSS_Encode
            var EM = EMSA_PSS_Encode(message, nlen - 1);

            // 2. RSA Decryption
            var signature = Decrypt(key.PubKey.N, key.PrivKey.D, EM.ToPositiveBigInteger());
            return new SignatureResult(new BitString(signature, nlen));
        }

        public override VerifyResult Verify(int nlen, BitString signature, IKeyPair key, BitString message)
        {
            // 1. Length Check
            if (signature.BitLength != nlen)
            {
                return new VerifyResult("Wrong signature bit length");
            }

            // 2. RSA Verification
            var EM_int = Encrypt(key.PubKey.N, key.PubKey.E, signature.ToPositiveBigInteger());
            var EM = new BitString(EM_int, nlen);

            // 3. EMSA_PSS_Verify
            return EMSA_PSS_Verify(message, EM, nlen - 1);
        }

        public override SignatureResult ModifyIRTrailerSign(int nlen, BitString message, IKeyPair key)
        {
            var emBits = nlen - 1;
            var mHash = Hash(message);
            var emLen = emBits.CeilingDivide(8);

            // All byte values
            if (emLen < mHash.BitLength / 8 + _saltLen + 2)
            {
                throw new Exception("Encoding error, RSA-SigGen-PSS");
            }

            var salt = _entropy.GetEntropy(_saltLen * 8);
            var mPrime = BitString.Zeroes(64);
            mPrime = BitString.ConcatenateBits(mPrime, mHash);
            mPrime = BitString.ConcatenateBits(mPrime, salt);

            var H = Hash(mPrime);

            // All bit values
            var PS = BitString.Zeroes(emLen * 8 - _saltLen * 8 - H.BitLength - 2 * 8);

            var DB = PS.GetDeepCopy();
            DB = BitString.ConcatenateBits(DB, _01);
            DB = BitString.ConcatenateBits(DB, salt);

            // All bit values
            var dbMask = MGF(H, emLen * 8 - H.BitLength - 1 * 8);
            var maskedDB = BitString.XOR(DB, dbMask);

            // Set leftmost bits to 0
            for (var i = 0; i < 8 * emLen - emBits; i++)
            {
                maskedDB.Set(maskedDB.BitLength - i - 1, false);
            }

            var EM = maskedDB.GetDeepCopy();
            EM = BitString.ConcatenateBits(EM, H);
            EM = BitString.ConcatenateBits(EM, new BitString("4C"));    // ERROR: should be BC

            // 2. RSA Decryption
            var signature = Decrypt(key.PubKey.N, key.PrivKey.D, EM.ToPositiveBigInteger());
            return new SignatureResult(new BitString(signature));
        }

        public override SignatureResult MoveIRSign(int nlen, BitString message, IKeyPair key)
        {
            var _rand = new Random800_90();
            var emBits = nlen - 1;
            var mHash = Hash(message);
            var emLen = emBits.CeilingDivide(8);

            // All byte values
            if (emLen < mHash.BitLength / 8 + _saltLen + 2)
            {
                throw new Exception("Encoding error, RSA-SigGen-PSS");
            }

            var salt = _entropy.GetEntropy(_saltLen * 8);
            var mPrime = BitString.Zeroes(64);
            mPrime = BitString.ConcatenateBits(mPrime, mHash);
            mPrime = BitString.ConcatenateBits(mPrime, salt);

            var H = Hash(mPrime);

            // All bit values
            var PS = BitString.Zeroes(emLen * 8 - _saltLen * 8 - H.BitLength - 2 * 8);

            var DB = PS.GetDeepCopy();
            DB = BitString.ConcatenateBits(DB, _01);
            DB = BitString.ConcatenateBits(DB, salt);

            // All bit values
            var dbMask = MGF(H, emLen * 8 - H.BitLength - 1 * 8);
            var maskedDB = BitString.XOR(DB, dbMask);

            // Set leftmost bits to 0
            for (var i = 0; i < 8 * emLen - emBits; i++)
            {
                maskedDB.Set(maskedDB.BitLength - i - 1, false);
            }

            var EM = maskedDB.Substring(0, maskedDB.BitLength - 8);     // ERROR: not all the maskedDB bits
            EM = BitString.ConcatenateBits(EM, H);
            EM = BitString.ConcatenateBits(EM, _rand.GetRandomBitString(8));    // ERROR: putting bits back to match length
            EM = BitString.ConcatenateBits(EM, _bc);

            // 2. RSA Decryption
            var signature = Decrypt(key.PubKey.N, key.PrivKey.D, EM.ToPositiveBigInteger());
            return new SignatureResult(new BitString(signature, nlen));
        }
    }
}
