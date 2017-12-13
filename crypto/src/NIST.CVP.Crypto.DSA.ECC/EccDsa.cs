using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class EccDsa : IDsaEcc
    {
        public ISha Sha { get; }

        private IEntropyProviderFactory _entropyFactory = new EntropyProviderFactory();
        private IEntropyProvider _entropyProvider;

        public EccDsa(ISha sha, EntropyProviderTypes entropyType = EntropyProviderTypes.Random)
        {
            Sha = sha;
            _entropyProvider = _entropyFactory.GetEntropyProvider(entropyType);
        }

        public EccDsa(EntropyProviderTypes entropyType)
        {
            _entropyProvider = _entropyFactory.GetEntropyProvider(entropyType);
        }

        public void AddEntropy(BigInteger entropy)
        {
            _entropyProvider.AddEntropy(entropy);
        }

        public EccDomainParametersGenerateResult GenerateDomainParameters(EccDomainParametersGenerateRequest generateRequest)
        {
            throw new NotImplementedException();
        }

        public EccDomainParametersValidateResult ValidateDomainParameters(EccDomainParametersValidateRequest domainParameters)
        {
            throw new NotImplementedException();
        }

        public EccKeyPairGenerateResult GenerateKeyPair(EccDomainParameters domainParameters)
        {
            // Generate random number d [1, n - 2]
            var d = _entropyProvider.GetEntropy(1, domainParameters.CurveE.OrderN - 1);

            // Compute Q such that Q = d * G
            var Q = domainParameters.CurveE.Multiply(domainParameters.CurveE.BasePointG, d);

            // Return key pair (Q, d)
            return new EccKeyPairGenerateResult(new EccKeyPair(Q, d));
        }

        public EccKeyPairValidateResult ValidateKeyPair(EccDomainParameters domainParameters, EccKeyPair keyPair)
        {
            // If Q == O, invalid
            if (keyPair.PublicQ.Infinity)
            {
                return new EccKeyPairValidateResult("Q cannot be infinity");
            }

            // If Q is not a valid point (x, y are within the field), invalid
            if (!domainParameters.CurveE.PointExistsInField(keyPair.PublicQ))
            {
                return new EccKeyPairValidateResult("Q is out of range of the field");
            }

            // If Q is not a valid point on the specific curve, invalid
            if (!domainParameters.CurveE.PointExistsOnCurve(keyPair.PublicQ))
            {
                return new EccKeyPairValidateResult("Q does not lie on the curve");
            }

            // If n * Q == 0, valid
            // This is fast because the scalar (n) is taken modulo n... so it's 0
            if (domainParameters.CurveE.Multiply(keyPair.PublicQ, domainParameters.CurveE.OrderN).Infinity)
            {
                return new EccKeyPairValidateResult();
            }

            // Otherwise invalid
            return new EccKeyPairValidateResult("n * Q must equal infinity");
        }

        public EccSignatureResult Sign(EccDomainParameters domainParameters, EccKeyPair keyPair, BitString message)
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
            var hashDigest = Sha.HashMessage(message).Digest;
            var e = hashDigest.MSBSubstring(0, bitsOfDigestNeeded).ToPositiveBigInteger();

            var s = (kInverse * (e + keyPair.PrivateD * r)).PosMod(domainParameters.CurveE.OrderN);

            // Return pair (r, s)
            return new EccSignatureResult(new EccSignature(r, s));
        }

        public EccVerificationResult Verify(EccDomainParameters domainParameters, EccKeyPair keyPair, BitString message, EccSignature signature)
        {
            // Check r and s to be within the interval [1, n-1]
            if (signature.R < 1 || signature.R > domainParameters.CurveE.OrderN - 1 || signature.S < 1 || signature.S > domainParameters.CurveE.OrderN)
            {
                return new EccVerificationResult("signature values not within the necessary interval");
            }

            // Hash message e = H(m)
            var bitsOfDigestNeeded = System.Math.Min(domainParameters.CurveE.OrderN.ExactBitLength(), Sha.HashFunction.OutputLen);
            var e = Sha.HashMessage(message).Digest.MSBSubstring(0, bitsOfDigestNeeded).ToPositiveBigInteger();

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
                return new EccVerificationResult();
            }
            else
            {
                return new EccVerificationResult("v did not match r, signature not valid");
            }
        }

        // Both secret generation methods exist, but we don't have a reason to use them. No need to worry about them.
        // These won't work well when the d value is provided instead of randomly generated because what comes out
        // of the entropy provider is modified before becoming d
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
