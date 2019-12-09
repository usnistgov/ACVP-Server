using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.SafePrimes
{
    public class SafePrimesFfc : IDsaFfc
    {
        public ISha Sha { get; }
        public FfcDomainParametersGenerateResult GenerateDomainParameters(FfcDomainParametersGenerateRequest generateRequest)
        {
            throw new System.NotImplementedException();
        }

        public FfcDomainParametersValidateResult ValidateDomainParameters(FfcDomainParametersValidateRequest domainParameters)
        {
            throw new System.NotImplementedException();
        }

        public FfcKeyPairGenerateResult GenerateKeyPair(FfcDomainParameters domainParameters)
        {
            var N = new BitString(domainParameters.Q).BitLength;

            var rand = new Random800_90();
            var c = rand.GetRandomBitString(N + 64).ToPositiveBigInteger();

            var x = (c % (domainParameters.Q - 1)) + 1;
            var y = BigInteger.ModPow(domainParameters.G, x, domainParameters.P);

            return new FfcKeyPairGenerateResult(new FfcKeyPair(x, y));
        }

        public FfcKeyPairValidateResult ValidateKeyPair(FfcDomainParameters domainParameters, FfcKeyPair keyPair)
        {
            if (keyPair.PrivateKeyX <= 0 || keyPair.PrivateKeyX >= domainParameters.Q)
            {
                return new FfcKeyPairValidateResult("Invalid key pair, x must satisfy 0 < x < q");
            }

            if (keyPair.PublicKeyY == BigInteger.ModPow(domainParameters.G, keyPair.PrivateKeyX, domainParameters.P))
            {
                return new FfcKeyPairValidateResult();
            }
            else
            {
                return new FfcKeyPairValidateResult("Invalid key pair, y != g^x mod p");
            }
        }

        public FfcSignatureResult Sign(FfcDomainParameters domainParameters, FfcKeyPair keyPair, BitString message,
            bool skipHash = false)
        {
            throw new System.NotImplementedException();
        }

        public FfcVerificationResult Verify(FfcDomainParameters domainParameters, FfcKeyPair keyPair, BitString message,
            FfcSignature signature, bool skipHash = false)
        {
            throw new System.NotImplementedException();
        }
    }
}