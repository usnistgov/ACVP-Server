using System;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.DSA.Ed
{
    public class EdDsa : IDsaEd
    {
        public ISha Sha { get; }

        private readonly IEntropyProviderFactory _entropyFactory = new EntropyProviderFactory();
        private readonly IEntropyProvider _entropyProvider;
        private readonly int _b;
        private readonly int _n;
        private readonly int _c;

        public EdDsa(ISha sha, int b, int n, int c, EntropyProviderTypes entropyType = EntropyProviderTypes.Random)
        {
            Sha = sha;
            _b = b;
            _n = n;
            _c = c;
            _entropyProvider = _entropyFactory.GetEntropyProvider(entropyType);
        }

        public EdDsa(EntropyProviderTypes entropyType)
        {
            _entropyProvider = _entropyFactory.GetEntropyProvider(entropyType);
        }

        public void AddEntropy(BigInteger entropy)
        {
            _entropyProvider.AddEntropy(entropy);
        }

        public EdDomainParametersGenerateResult GenerateDomainParameters(EdDomainParametersGenerateRequest generateRequest)
        {
            throw new NotImplementedException();
        }

        public EdDomainParametersValidateResult ValidateDomainParameters(EdDomainParametersValidateRequest domainParameters)
        {
            throw new NotImplementedException();
        }

        public EdKeyPairGenerateResult GenerateKeyPair(EdDomainParameters domainParameters)
        {
            // Generate random number k [1, n - 2]
            var k = _entropyProvider.GetEntropy(1, domainParameters.CurveE.OrderN - 1);

            // Hash the private key
            var h = Sha.HashMessage(new BitString(k, _b)).Digest.MSBSubstring(0, 2 * _b - 1);

            // Format
            for (int i = 0; i < _c; i++)
            {
                h.Bits.Set(i, false);
            }
            for (int i = _n; i < h.Bits.Length; i++)
            {
                h.Bits.Set(i, false);
            }

            // Determine s
            var s = BigInteger.Pow(2, _n) + h.ToPositiveBigInteger();

            // Compute Q such that Q = d * G
            var Q = domainParameters.CurveE.Multiply(domainParameters.CurveE.BasePointG, s);

            // Return key pair (Q, d)
            return new EdKeyPairGenerateResult(new EdKeyPair(domainParameters.CurveE.Encode(Q), k));
        }

        public EdKeyPairValidateResult ValidateKeyPair(EdDomainParameters domainParameters, EdKeyPair keyPair)
        {
            // If Q == (0, 1), invalid
            if (keyPair.PublicQ.Equals(new EdPoint(0, 1)))
            {
                return new EdKeyPairValidateResult("Q cannot be neutral element");
            }

            // If Q is not a valid point (x, y are within the field), invalid
            if (!domainParameters.CurveE.PointExistsInField(keyPair.PublicQ))
            {
                return new EdKeyPairValidateResult("Q is out of range of the field");
            }

            // If Q is not a valid point on the specific curve, invalid
            if (!domainParameters.CurveE.PointExistsOnCurve(keyPair.PublicQ))
            {
                return new EdKeyPairValidateResult("Q does not lie on the curve");
            }

            // If n * Q == 0, valid
            // This is fast because the scalar (n) is taken modulo n... so it's 0
            if (domainParameters.CurveE.Multiply(keyPair.PublicQ, domainParameters.CurveE.OrderN).Equals(new EdPoint(0, 1)))
            {
                return new EdKeyPairValidateResult();
            }

            // Otherwise invalid
            return new EdKeyPairValidateResult("n * Q must equal infinity");
        }

        public EdSignatureResult Sign(EdDomainParameters domainParameters, EdKeyPair keyPair, BitString message, bool skipHash = false)
        {
            // Generate random number k [1, n-1]
            var k = _entropyProvider.GetEntropy(1, domainParameters.CurveE.OrderN - 1);

            // Compute point (x, y) = k * G
            var point = domainParameters.CurveE.Multiply(domainParameters.CurveE.BasePointG, k);

            // Represent x as an integer j
            var j = point.X;

            // Compute r = j mod n
            var r = j % domainParameters.CurveE.OrderN;

            // Compute s = k^-1 (e + d*r) mod n, where e = H(m) as an integer
            var kInverse = k.ModularInverse(domainParameters.CurveE.OrderN);

            var bitsOfDigestNeeded = System.Math.Min(domainParameters.CurveE.OrderN.ExactBitLength(), Sha.HashFunction.OutputLen);

            // Determine whether to hash or skip the hash step for component test
            var hashDigest = skipHash ? message : Sha.HashMessage(message).Digest;

            var e = hashDigest.MSBSubstring(0, bitsOfDigestNeeded).ToPositiveBigInteger();

            var s = (kInverse * (e + keyPair.PrivateD * r)).PosMod(domainParameters.CurveE.OrderN);

            // Return pair (r, s)
            return new EdSignatureResult(new EdSignature(r, s));
        }

        public EdVerificationResult Verify(EdDomainParameters domainParameters, EdKeyPair keyPair, BitString message, EdSignature signature, bool skipHash = false)
        {
            // Check r and s to be within the interval [1, n-1]
            if (signature.R < 1 || signature.R > domainParameters.CurveE.OrderN - 1 || signature.S < 1 || signature.S > domainParameters.CurveE.OrderN)
            {
                return new EdVerificationResult("signature values not within the necessary interval");
            }

            // Hash message e = H(m)
            var bitsOfDigestNeeded = System.Math.Min(domainParameters.CurveE.OrderN.ExactBitLength(), Sha.HashFunction.OutputLen);

            // Determine whether to hash or skip the hash step for component test
            var hashDigest = skipHash ? message : Sha.HashMessage(message).Digest;

            var e = hashDigest.MSBSubstring(0, bitsOfDigestNeeded).ToPositiveBigInteger();

            // Compute u1 = e * s^-1 (mod n)
            var sInverse = signature.S.ModularInverse(domainParameters.CurveE.OrderN);
            var u1 = (e * sInverse) % domainParameters.CurveE.OrderN;

            // Compute u2 = r * s^-1 (mod n)
            var u2 = (signature.R * sInverse) % domainParameters.CurveE.OrderN;

            // Compute point R = u1 * G + u2 * Q, if R is infinity, return invalid
            var u1TimesG = domainParameters.CurveE.Multiply(domainParameters.CurveE.BasePointG, u1);
            var u2TimesQ = domainParameters.CurveE.Multiply(keyPair.PublicQ, u2);
            var pointR = domainParameters.CurveE.Add(u1TimesG, u2TimesQ);

            // Convert xR to an integer j
            var j = pointR.X;

            // Compute v = j (mod n)
            var v = j % domainParameters.CurveE.OrderN;

            // If v == r, return valid, otherwise invalid
            if (v == signature.R)
            {
                return new EdVerificationResult();
            }
            else
            {
                return new EdVerificationResult("v did not match r, signature not valid");
            }
        }

        // Both secret generation methods exist, but we don't have a reason to use them. No need to worry about them.
        // These won't work well when the d value is provided via entropy provider instead of randomly generated because 
        // what comes out of the entropy provider is modified before becoming d
        private BigInteger GetSecretViaExtraRandomBits(BigInteger N)
        {
            var bitLength = N.ExactBitLength();
            var c = _entropyProvider.GetEntropy(bitLength + 64).ToPositiveBigInteger();
            var d = (c % (N - 1)) + 1;
            return d;
        }

        private BigInteger GetSecretViaTestingCandidates(BigInteger N)
        {
            var bitLength = N.ExactBitLength();

            BigInteger c;
            do
            {
                c = _entropyProvider.GetEntropy(bitLength).ToPositiveBigInteger();
            } while (c > N - 2);

            var d = c + 1;
            return d;
        }
    }
}
