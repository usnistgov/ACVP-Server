using System.Numerics;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public class FfcDsa : IDsaFfc
    {
        public ISha Sha { get; }

        private PQGeneratorValidatorFactory _pqGeneratorFactory = new PQGeneratorValidatorFactory();
        private GGeneratorValidatorFactory _gGeneratorFactory = new GGeneratorValidatorFactory();

        public FfcDsa(ISha sha)
        {
            Sha = sha;
        }

        public FfcDomainParametersGenerateResult GenerateDomainParameters(FfcDomainParametersGenerateRequest generateRequest)
        {
            // Generate p and q
            var pqGenerator = _pqGeneratorFactory.GetGeneratorValidator(generateRequest.PrimeGen, Sha);
            var pqResult = pqGenerator.Generate(generateRequest.PLength, generateRequest.QLength, generateRequest.SeedLength);
            if (!pqResult.Success)
            {
                return new FfcDomainParametersGenerateResult($"Failed to generate p and q with error: {pqResult.ErrorMessage}");
            }

            // Generate g
            var gGenerator = _gGeneratorFactory.GetGeneratorValidator(generateRequest.GeneratorGen, Sha);
            var gResult = gGenerator.Generate(pqResult.P, pqResult.Q);
            if (!gResult.Success)
            {
                return new FfcDomainParametersGenerateResult($"Failed to generate g with error: {gResult.ErrorMessage}");
            }

            var domainParameters = new FfcDomainParameters(pqResult.P, pqResult.Q, gResult.G);
            return new FfcDomainParametersGenerateResult(domainParameters, pqResult.Seed, pqResult.Counter);
        }

        public FfcDomainParametersValidateResult ValidateDomainParameters(FfcDomainParametersValidateRequest validateRequest)
        {
            // Validate p and q
            var pqGenerator = _pqGeneratorFactory.GetGeneratorValidator(validateRequest.PrimeGen, Sha);
            var pqResult = pqGenerator.Validate();
            if (!pqResult.Success)
            {
                return new FfcDomainParametersValidateResult($"Failed to generate p and q with error: {pqResult.ErrorMessage}");
            }

            // Validate g
            var gGenerator = _gGeneratorFactory.GetGeneratorValidator(validateRequest.GeneratorGen, Sha);
            var gResult = gGenerator.Validate();
            if (!gResult.Success)
            {
                return new FfcDomainParametersValidateResult($"Failed to generate g with error: {gResult.ErrorMessage}");
            }

            return new FfcDomainParametersValidateResult();
        }

        /// <summary>
        /// B.1.1 from FIPS 186-4. This is equivalent to B.1.2, the other KeyGeneration method.
        /// </summary>
        /// <param name="domainParameters"></param>
        /// <returns></returns>
        public FfcKeyPairGenerateResult GenerateKeyPair(FfcDomainParameters domainParameters)
        {
            var L = new BitString(domainParameters.P).BitLength;
            var N = new BitString(domainParameters.Q).BitLength;

            // Shouldn't really be necessary but just in case
            if (!DSAHelper.VerifyLenPair(L, N))
            {
                return new FfcKeyPairGenerateResult("Invalid L, N pair");
            }

            var rand = new Random800_90();
            var c = rand.GetRandomBitString(N + 64).ToPositiveBigInteger();

            var x = (c % (domainParameters.Q - 1)) + 1;
            var y = BigInteger.ModPow(domainParameters.G, x, domainParameters.P);

            return new FfcKeyPairGenerateResult(new FfcKeyPair(x, y));
        }

        public FfcKeyPairValidateResult ValidateKeyPair(FfcDomainParameters domainParameters, FfcKeyPair keyPair)
        {
            if (keyPair.PublicKeyY == BigInteger.ModPow(domainParameters.G, keyPair.PrivateKeyX, domainParameters.P))
            {
                return new FfcKeyPairValidateResult();
            }
            else
            {
                return new FfcKeyPairValidateResult("Invalid key pair, y != g^x mod p");
            }
        }
    }
}
