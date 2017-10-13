using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

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
            if (domainParameters.CurveE.CurveType == CurveType.Prime)
            {
                if (keyPair.PublicQ.X < 0 || keyPair.PublicQ.X > domainParameters.CurveE.FieldSizeQ - 1)
                {
                    return new EccKeyPairValidateResult("Qx is out of range for prime curve");
                }

                if (keyPair.PublicQ.Y < 0 || keyPair.PublicQ.Y > domainParameters.CurveE.FieldSizeQ - 1)
                {
                    return new EccKeyPairValidateResult("Qy is out of range for prime curve");
                }
            }
            else if (domainParameters.CurveE.CurveType == CurveType.Binary)
            {
                var xBitString = new BitString(keyPair.PublicQ.X);
                var yBitString = new BitString(keyPair.PublicQ.Y);
                var m = BigInteger.Log(domainParameters.CurveE.FieldSizeQ, 2);     // We know that q = 2^m, so m = log_2(q)

                if (xBitString.BitLength == m || yBitString.BitLength == m)
                {
                    return new EccKeyPairValidateResult("x and y must not have bitlength m");
                }
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
            return new EccKeyPairValidateResult("nQ must equal infinity");
        }

        public EccSignatureResult Sign(EccDomainParameters domainParameters, EccKeyPair keyPair, BitString message)
        {
            // Generate random number k [1, n-1]

            // Compute point (x, y) = k * G

            // Represent x as an integer j

            // Compute r = j mod n

            // Compute s = k^-1 (e + d*r) mod n, where e = H(m) as an integer

            // Return pair (r, s)
            throw new NotImplementedException();
        }

        public EccVerificationResult Verify(EccDomainParameters domainParameters, EccKeyPair keyPair, BitString message, EccSignature signature)
        {
            throw new NotImplementedException();
        }
    }
}
