﻿using System;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DSA.ECC
{
    public class EccDsa : IDsaEcc
    {
        public ISha Sha { get; }

        private readonly IEccNonceProvider _nonceProvider;

        private readonly IEntropyProviderFactory _entropyFactory = new EntropyProviderFactory();
        private readonly IEntropyProvider _entropyProvider;

        // Used for both signatures and keys
        public EccDsa(ISha sha, IEccNonceProvider nonceProvider, EntropyProviderTypes entropyType)
        {
            Sha = sha;
            _nonceProvider = nonceProvider;
            _entropyProvider = _entropyFactory.GetEntropyProvider(entropyType);
        }

        // Used for ONLY signatures
        public EccDsa(ISha sha, IEccNonceProvider nonceProvider)
        {
            Sha = sha;
            _nonceProvider = nonceProvider;
        }

        // Used ONLY for verifying signatures
        public EccDsa(ISha sha)
        {
            Sha = sha;
        }

        // Used for ONLY keys
        public EccDsa(IEntropyProvider entropyProvider)
        {
            _entropyProvider = entropyProvider;
        }

        // Used for ONLY key verification
        public EccDsa()
        {

        }

        /// <summary>
        /// KAS constructor
        /// </summary>
        /// <param name="sha">The SHA instance.</param>
        /// <param name="entropyProvider">An entropy provider.</param>
        public EccDsa(ISha sha, IEntropyProvider entropyProvider)
        {
            Sha = sha;
            _entropyProvider = entropyProvider;
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
            // Generate random number d [1, n - 1]
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

        public EccSignatureResult Sign(EccDomainParameters domainParameters, EccKeyPair keyPair, BitString message, bool skipHash = false)
        {
            int shaOutputLen;

            // FIPS 186-5 dictates that an outputLen of 256 shall be used when SHAKE128 is used
            // and an outputLen of 512 when SHAKE256 is in use.
            if (Sha.HashFunction.Mode == ModeValues.SHAKE)
            {
                shaOutputLen = Sha.HashFunction.DigestSize == DigestSizes.d128 ? 256 : 512;
            }
            else // case: Sha2 or Sha3
            {
                shaOutputLen = Sha.HashFunction.OutputLen;
            }
            
            var bitsOfDigestNeeded = System.Math.Min(domainParameters.CurveE.OrderN.ExactBitLength(), shaOutputLen);

            var hashDigest = skipHash ? message : Sha.HashMessage(message, shaOutputLen).Digest;

            var e = hashDigest.MSBSubstring(0, bitsOfDigestNeeded).ToPositiveBigInteger();

            // Generate random number k [1, n-1]
            var k = _nonceProvider.GetNonce(keyPair.PrivateD, hashDigest, domainParameters.CurveE.OrderN);
            //var k = _entropyProvider.GetEntropy(1, domainParameters.CurveE.OrderN - 1);

            // Compute point (x, y) = k * G
            var point = domainParameters.CurveE.Multiply(domainParameters.CurveE.BasePointG, k);

            // Represent x as an integer j
            var j = point.X;

            // Compute r = j mod n
            var r = j % domainParameters.CurveE.OrderN;

            // Compute s = k^-1 (e + d*r) mod n, where e = H(m) as an integer
            var kInverse = k.ModularInverse(domainParameters.CurveE.OrderN);
            var s = (kInverse * (e + keyPair.PrivateD * r)).PosMod(domainParameters.CurveE.OrderN);

            // Return pair (r, s)
            return new EccSignatureResult(k, new EccSignature(r, s));
        }

        public EccVerificationResult Verify(EccDomainParameters domainParameters, EccKeyPair keyPair, BitString message, EccSignature signature, bool skipHash = false)
        {
            // Check r and s to be within the interval [1, n-1]
            if (signature.R < 1 || signature.R > domainParameters.CurveE.OrderN - 1 || signature.S < 1 || signature.S > domainParameters.CurveE.OrderN)
            {
                return new EccVerificationResult("signature values not within the necessary interval");
            }

            int shaOutputLen;

            // FIPS 186-5 dictates that an outputLen of 256 shall be used when SHAKE128 is used
            // and an outputLen of 512 when SHAKE256 is in use.
            if (Sha.HashFunction.Mode == ModeValues.SHAKE)
            {
                shaOutputLen = Sha.HashFunction.DigestSize == DigestSizes.d128 ? 256 : 512;
            }
            else // case: Sha2 or Sha3
            {
                shaOutputLen = Sha.HashFunction.OutputLen;
            }
            
            // Hash message e = H(m)
            var bitsOfDigestNeeded = System.Math.Min(domainParameters.CurveE.OrderN.ExactBitLength(), shaOutputLen);

            // Determine whether to hash or skip the hash step for component test
            var hashDigest = skipHash ? message : Sha.HashMessage(message, shaOutputLen).Digest;

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
                return new EccVerificationResult();
            }
            else
            {
                return new EccVerificationResult("v did not match r, signature not valid");
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
