using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS
{
    public class Lmots
    {
        #region Fields

        private readonly BitString D_PBLC = new BitString("8080");
        private readonly BitString D_MESG = new BitString("8181");
        private readonly int _n;
        private readonly int _w;
        private readonly int _p;
        private readonly int _ls;
        private readonly int _siglen;
        private readonly BitString _typecode;
        private readonly IEntropyProviderFactory _entropyFactory = new EntropyProviderFactory();
        private readonly IEntropyProvider _entropyProvider;
        private readonly bool _isRandom;
        private readonly ISha _sha256;

        #endregion Fields

        #region Constructors

        public Lmots(LmotsType type, EntropyProviderTypes entropyType = EntropyProviderTypes.Random)
        {
            var (n, w, p, ls, siglen, typecode) = LmotsModeMapping.GetValsFromType(type);
            _n = n;
            _w = w;
            _p = p;
            _ls = ls;
            _siglen = siglen;
            _typecode = typecode;
            _entropyProvider = _entropyFactory.GetEntropyProvider(entropyType);
            _isRandom = entropyType == EntropyProviderTypes.Random;
            _sha256 = new NativeFastSha2_256();
        }

        #endregion Constructors

        #region Accessors

        public int GetN()
        {
            return _n;
        }

        public int GetP()
        {
            return _p;
        }

        public int GetW()
        {
            return _w;
        }

        #endregion Accessors

        #region Helpers

        // As described on page 8 of RFC 8554
        private BigInteger Coef(BitString S, int i, int w)
        {
            var resultlhs = new BitString(new BigInteger((1 << w) - 1));

            var resultrhs = new BitString(S.MSBSubstring(i * w / 8 * 8, 8).ToPositiveBigInteger() >> (8 - (w * (i % (8 / w)) + w)));

            return resultlhs.AND(resultrhs).ToPositiveBigInteger();
        }

        // As described on page 16 of RFC 8554
        private BitString CheckSum(BitString S)
        {
            var sum = new BigInteger(0);

            for (var i = 0; i < (_n * 8 / _w); i++)
            {
                sum += (1 << _w) - 1 - Coef(S, i, _w);
            }

            return new BitString(sum << _ls);
        }

        #endregion Helpers

        #region Key Generation

        // Uses algorithm described in Appendix A of rfc 8554
        public BitString GenerateLmotsPrivateKey(BitString q, BitString I, BitString seed = null)
        {
            if (_isRandom)
            {
                var rand = _entropyProvider.GetEntropy(_n * 8 * _p);

                return _typecode.ConcatenateBits(q).ConcatenateBits(I).ConcatenateBits(rand);
            }
            else
            {
                var priv = LmsDllLoader.GenPrivLmots(_n, _p, q.ToBytes(), I.ToBytes(), seed.ToBytes(), _typecode.ToBytes());

                return new BitString(priv);
            }
        }

        public byte[] GenerateLmotsPublicKey(BitString privateKey)
        {
            return LmsDllLoader.GenPubLmots(_n, _p, _w, privateKey.ToBytes());
        }

        #endregion Key Generation

        #region Signature Generation

        // To generate the randomized value C, we will adapt algorithm A, by using the I value of the LMS tree,
        // the q value to be the LMS index, and the i value to be 65533
        // Algorithm A:
        // x_q[i] = H(I || u32str(q) || u16str(i) || u8str(0xff) || SEED).
        public BitString GenerateLmotsSignature(BitString msg, BitString privateKey, BitString seed = null)
        {
            if (_isRandom)
            {
                var C = _entropyProvider.GetEntropy(_n * 8);

                var sig = LmsDllLoader.GenSigLmotsNonDeterministic(_n, _p, _w, _ls, msg.ToBytes().Length, privateKey.ToBytes(), C.ToBytes(), msg.ToBytes());

                return new BitString(sig);
            }
            else
            {
                var sig = LmsDllLoader.GenSigLmots(_n, _p, _w, _ls, msg.ToBytes().Length, privateKey.ToBytes(), seed.ToBytes(), msg.ToBytes());

                return new BitString(sig);
            }
        }

        #endregion Signature Generation

        #region Signature Verification

        public LmotsVerificationResult VerifyLmotsSignature(BitString sig, BitString publicKey, BitString msg)
        {
            // 1. If the public key is not at least four bytes long, return INVALID.
            if (publicKey.BitLength < 32)
            {
                return new LmotsVerificationResult("Validation failed. Public key wrong length.");
            }

            // 2.  Parse pubtype, I, q, and K from the public key as follows:
            // a. pubtype = strTou32(first 4 bytes of public key)
            var pubType = publicKey.MSBSubstring(0, 32);

            // b. Set n according to the pubkey and Table 1; if the public key is not exactly 24 + n bytes long, return INVALID.
            var n = LmotsModeMapping.GetNFromCode(pubType);
            if (publicKey.BitLength != (n + 24) * 8)
            {
                return new LmotsVerificationResult("Validation failed. Public key wrong length.");
            }

            // c. I = next 16 bytes of public key
            var I = publicKey.MSBSubstring(32, 128);

            // d. q = strTou32(next 4 bytes of public key)
            var q = publicKey.MSBSubstring(160, 32);

            // e. K = next n bytes of public key
            var K = publicKey.MSBSubstring(192, n * 8);

            // 3. Compute the public key candidate Kc from the signature, message, pubtype, and the identifiers I and q 
            //    obtained from the public key, using Algorithm 4b.If Algorithm 4b returns INVALID, then return INVALID.
            var Kc = Algorithm4b(sig, msg, pubType, I, q);
            if (Kc == null)
            {
                return new LmotsVerificationResult("Validation failed. Computing Kc failed.");
            }

            // 4. If Kc is equal to K, return VALID; otherwise, return INVALID.
            if (K.Equals(Kc))
            {
                return new LmotsVerificationResult();
            }
            else
            {
                return new LmotsVerificationResult("Validation failed. Signature does not match.");
            }
        }

        // Computes Kc as described in rfc 8554
        public BitString Algorithm4b(BitString sig, BitString msg, BitString pubType, BitString I, BitString q)
        {
            // 1. If the signature is not at least four bytes long, return INVALID.
            if (sig.BitLength < 32)
            {
                return null;
            }

            // 2. Parse sigtype, C, and y from the signature as follows:
            // a. sigtype = strTou32(first 4 bytes of signature)
            var sigType = sig.MSBSubstring(0, 32);

            // b. If sigtype is not equal to pubtype, return INVALID.
            if (!pubType.Equals(sigType))
            {
                return null;
            }

            // c. Set n and p according to the pubtype and Table 1; if the signature is not exactly 4 + n * (p + 1) 
            //    bytes long, return INVALID.
            var p = LmotsModeMapping.GetPFromCode(sigType);
            var n = LmotsModeMapping.GetNFromCode(sigType);
            if (sig.BitLength != (4 + (n * (p + 1))) * 8)
            {
                return null;
            }

            // d. C = next n bytes of signature
            var C = sig.MSBSubstring(32, n * 8);

            // e. y[0] = next n bytes of signature
            //    y[1] = next n bytes of signature
            //    ...
            //    y[p - 1] = next n bytes of signature
            var y = sig.MSBSubstring((n * 8) + 32, p * n * 8);

            // 3. Compute the string Kc as described in rfc 8554
            var Q = _sha256.HashMessage(I
                .ConcatenateBits(q)
                .ConcatenateBits(D_MESG)
                .ConcatenateBits(C)
                .ConcatenateBits(msg)).Digest;
            var cksmQ = CheckSum(Q);
            var QcksmQ = Q.ConcatenateBits(cksmQ);

            var z = LmsDllLoader.GenZ(_p, _n, _w, y.ToBytes(), QcksmQ.ToBytes(), I.ToBytes(), q.ToBytes());

            var concatenated = I.ConcatenateBits(q).ConcatenateBits(D_PBLC);

            concatenated = concatenated.ConcatenateBits(new BitString(z));

            var Kc = _sha256.HashMessage(concatenated).Digest;

            // 4. Return Kc.
            return Kc;
        }

        #endregion Signature Verification

    }

}
