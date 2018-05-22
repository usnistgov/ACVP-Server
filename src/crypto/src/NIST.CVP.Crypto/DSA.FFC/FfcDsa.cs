using System.Numerics;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public class FfcDsa : IDsaFfc
    {
        public ISha Sha { get; }

        private IEntropyProviderFactory _entropyFactory = new EntropyProviderFactory();
        private IEntropyProvider _entropyProvider;

        private PQGeneratorValidatorFactory _pqGeneratorFactory = new PQGeneratorValidatorFactory();
        private GGeneratorValidatorFactory _gGeneratorFactory = new GGeneratorValidatorFactory();

        public FfcDsa(ISha sha, EntropyProviderTypes entropyType = EntropyProviderTypes.Random)
        {
            Sha = sha;
            _entropyProvider = _entropyFactory.GetEntropyProvider(entropyType);
        }

        public void AddEntropy(BigInteger entropy)
        {
            _entropyProvider.AddEntropy(entropy);
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
            var gResult = gGenerator.Generate(pqResult.P, pqResult.Q, pqResult.Seed, generateRequest.Index);
            if (!gResult.Success)
            {
                return new FfcDomainParametersGenerateResult($"Failed to generate g with error: {gResult.ErrorMessage}");
            }

            var domainParameters = new FfcDomainParameters(pqResult.P, pqResult.Q, gResult.G);
            return new FfcDomainParametersGenerateResult(domainParameters, pqResult.Seed, pqResult.Count);
        }

        public FfcDomainParametersValidateResult ValidateDomainParameters(FfcDomainParametersValidateRequest validateRequest)
        {
            // Validate p and q
            var domainParams = validateRequest.PqgDomainParameters;
            var pqGenerator = _pqGeneratorFactory.GetGeneratorValidator(validateRequest.PrimeGen, Sha);
            var pqResult = pqGenerator.Validate(domainParams.P, domainParams.Q, validateRequest.Seed, validateRequest.Count);
            if (!pqResult.Success)
            {
                return new FfcDomainParametersValidateResult($"Failed to generate p and q with error: {pqResult.ErrorMessage}");
            }

            // Validate g
            var gGenerator = _gGeneratorFactory.GetGeneratorValidator(validateRequest.GeneratorGen, Sha);
            var gResult = gGenerator.Validate(domainParams.P, domainParams.Q, domainParams.G, validateRequest.Seed, validateRequest.Index);
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

        public FfcSignatureResult Sign(FfcDomainParameters domainParameters, FfcKeyPair keyPair, BitString message, bool skipHash = false)
        {
            BigInteger r, s;
            do
            {
                var k = _entropyProvider.GetEntropy(1, domainParameters.Q - 1);
                var kInv = k.ModularInverse(domainParameters.Q);

                r = BigInteger.ModPow(domainParameters.G, k, domainParameters.P) % domainParameters.Q;

                var zLen = System.Math.Min(Sha.HashFunction.OutputLen, new BitString(domainParameters.Q).BitLength);
                var z = BitString.MSBSubstring(Sha.HashMessage(message).Digest, 0, zLen).ToPositiveBigInteger();

                s = (kInv * (z + keyPair.PrivateKeyX * r)) % domainParameters.Q;

            } while (r == 0 || s == 0);

            return new FfcSignatureResult(new FfcSignature(r, s));
        }

        public FfcVerificationResult Verify(FfcDomainParameters domainParameters, FfcKeyPair keyPair, BitString message, FfcSignature signature, bool skipHash = false)
        {
            // 1
            if(signature.R < 0 || signature.R > domainParameters.Q)
            {
                return new FfcVerificationResult("Invalid r provided");
            }

            if(signature.S < 0 || signature.S > domainParameters.Q)
            {
                return new FfcVerificationResult("Invalid s provided");
            }

            // 2
            var w = signature.S.ModularInverse(domainParameters.Q);
            var zLen = System.Math.Min(Sha.HashFunction.OutputLen, new BitString(domainParameters.Q).BitLength);
            var z = BitString.MSBSubstring(Sha.HashMessage(message).Digest, 0, zLen).ToPositiveBigInteger();
            var u1 = (z * w) % domainParameters.Q;
            var u2 = (signature.R * w) % domainParameters.Q;

            // (g^u1 * y^u2) mod p == [(g^u1 mod p) * (y^u2 mod p)] mod p
            var v = ((BigInteger.ModPow(domainParameters.G, u1, domainParameters.P) * BigInteger.ModPow(keyPair.PublicKeyY, u2, domainParameters.P)) % domainParameters.P) % domainParameters.Q;

            // 3
            if(v != signature.R)
            {
                return new FfcVerificationResult("Invalid v, does not match provided r");
            }

            return new FfcVerificationResult();
        }
    }
}
