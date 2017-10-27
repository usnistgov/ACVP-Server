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
            // Generate random number d [1, n-1]
            var d = _entropyProvider.GetEntropy(1, domainParameters.CurveE.OrderN);

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
            var s = (NumberTheory.ModularInverse(k, domainParameters.CurveE.OrderN) * (Sha.HashMessage(message).ToBigInteger() + keyPair.PrivateD * r)).PosMod(domainParameters.CurveE.OrderN);

            // Return pair (r, s)
            return new EccSignatureResult(new EccSignature(r, s));
        }

        public EccVerificationResult Verify(EccDomainParameters domainParameters, EccKeyPair keyPair, BitString message, EccSignature signature)
        {
            // Check r and s to be within the interval [1, n-1]

            // Hash message e = H(m)

            // Compute u1 = e * s^-1 (mod n)

            // Compute u2 = r * s^-1 (mod n)

            // Compute point R = u1 * G + u2 * Q, if R is infinity, return invalid

            // Convert xR to an integer j

            // Compute v = j (mod n)

            // If v == r, return valid, otherwise invalid
            throw new NotImplementedException();
        }
    }
}
