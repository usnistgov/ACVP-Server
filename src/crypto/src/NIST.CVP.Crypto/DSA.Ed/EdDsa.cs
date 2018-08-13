using System;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.DSA.Ed
{
    public class EdDsa : IDsaEd
    {
        public ISha Sha => throw new NotImplementedException();

        private readonly IEntropyProviderFactory _entropyFactory = new EntropyProviderFactory();
        private readonly IEntropyProvider _entropyProvider;

        public EdDsa(EntropyProviderTypes entropyType = EntropyProviderTypes.Random)
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

            // 1. Hash the private key
            var sha = new SHA3.SHA3();
            var h = sha.HashMessage(domainParameters.Hash, new BitString(k, domainParameters.CurveE.VariableB)).Digest.MSBSubstring(0, 2 * domainParameters.CurveE.VariableB - 1);

            // 2. Prune the buffer
            // Currently this is accomplished using the specifications in IETF RFC 8032
            // Could switch over to how it is specified in SP186-5 in the future
            for (int i = 0; i < domainParameters.CurveE.VariableC; i++)
            {
                h.Bits.Set(i, false);
            }
            for (int i = domainParameters.CurveE.VariableN; i < h.Bits.Length; i++)
            {
                h.Bits.Set(i, false);
            }

            // 3. Determine s
            var s = BigInteger.Pow(2, domainParameters.CurveE.VariableN) + h.ToPositiveBigInteger();

            // 4. Compute Q such that Q = s * G
            var Q = domainParameters.CurveE.Multiply(domainParameters.CurveE.BasePointG, s);

            // Return key pair (Q, d)
            return new EdKeyPairGenerateResult(new EdKeyPair(domainParameters.CurveE.Encode(Q, domainParameters.CurveE.VariableB), k));
        }

        public EdKeyPairValidateResult ValidateKeyPair(EdDomainParameters domainParameters, EdKeyPair keyPair)
        {
            // If Q == (0, 1), invalid
            if (keyPair.PublicQ.Equals(new EdPoint(0, 1)))
            {
                return new EdKeyPairValidateResult("Q cannot be neutral element");
            }

            // If Q is not a valid point (x, y are within the field), invalid
            // could make this more efficient
            if (!domainParameters.CurveE.PointExistsInField(domainParameters.CurveE.Decode(keyPair.PublicQ, domainParameters.CurveE.VariableB)))
            {
                return new EdKeyPairValidateResult("Q is out of range of the field");
            }

            // If Q is not a valid point on the specific curve, invalid
            // could make this more efficient
            if (!domainParameters.CurveE.PointExistsOnCurve(domainParameters.CurveE.Decode(keyPair.PublicQ, domainParameters.CurveE.VariableB)))
            {
                return new EdKeyPairValidateResult("Q does not lie on the curve");
            }

            // If n * Q == 0, valid
            // This is fast because the scalar (n) is taken modulo n... so it's 0
            if (domainParameters.CurveE.Multiply(domainParameters.CurveE.Decode(keyPair.PublicQ, domainParameters.CurveE.VariableB), domainParameters.CurveE.OrderN).Equals(new EdPoint(0, 1)))
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
            var u2TimesQ = domainParameters.CurveE.Multiply(domainParameters.CurveE.Decode(keyPair.PublicQ, domainParameters.CurveE.VariableB), u2);
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
