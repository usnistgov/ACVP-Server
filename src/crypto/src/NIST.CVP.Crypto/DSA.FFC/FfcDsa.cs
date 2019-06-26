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

        private readonly IEntropyProviderFactory _entropyFactory = new EntropyProviderFactory();
        private readonly IEntropyProvider _entropyProvider;

        private readonly PQGeneratorValidatorFactory _pqGeneratorFactory = new PQGeneratorValidatorFactory();
        private readonly GGeneratorValidatorFactory _gGeneratorFactory = new GGeneratorValidatorFactory();

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
            var L = domainParameters.P.BitLength;
            var N = domainParameters.Q.BitLength;

            // Shouldn't really be necessary but just in case
            if (!DSAHelper.VerifyLenPair(L, N))
            {
                return new FfcKeyPairGenerateResult("Invalid L, N pair");
            }

            var rand = new Random800_90();
            var c = rand.GetRandomBitString(N + 64).ToPositiveBigInteger();

            var x = (c % (domainParameters.Q.ToPositiveBigInteger() - 1)) + 1;
            var y = BigInteger.ModPow(domainParameters.G.ToPositiveBigInteger(), x, domainParameters.P.ToPositiveBigInteger());

            return new FfcKeyPairGenerateResult(new FfcKeyPair(new BitString(x), new BitString(y)));
        }

        public FfcKeyPairValidateResult ValidateKeyPair(FfcDomainParameters domainParameters, FfcKeyPair keyPair)
        {
            var x = keyPair.PrivateKeyX.ToPositiveBigInteger();
            var y = keyPair.PublicKeyY.ToPositiveBigInteger();
            var p = domainParameters.P.ToPositiveBigInteger();
            var q = domainParameters.Q.ToPositiveBigInteger();
            var g = domainParameters.G.ToPositiveBigInteger();
            
            if (x <= 0 || x >= q)
            {
                return new FfcKeyPairValidateResult("Invalid key pair, x must satisfy 0 < x < q");
            }

            if (y == BigInteger.ModPow(g, x, p))
            {
                return new FfcKeyPairValidateResult();
            }

            return new FfcKeyPairValidateResult("Invalid key pair, y != g^x mod p");
        }

        public FfcSignatureResult Sign(FfcDomainParameters domainParameters, FfcKeyPair keyPair, BitString message, bool skipHash = false)
        {
            var x = keyPair.PrivateKeyX.ToPositiveBigInteger();
            var p = domainParameters.P.ToPositiveBigInteger();
            var q = domainParameters.Q.ToPositiveBigInteger();
            var g = domainParameters.G.ToPositiveBigInteger();
            
            BigInteger r, s;
            do
            {
                var k = _entropyProvider.GetEntropy(1, q - 1);
                var kInv = k.ModularInverse(q);

                r = BigInteger.ModPow(g, k, p) % q;

                var zLen = System.Math.Min(Sha.HashFunction.OutputLen, domainParameters.Q.BitLength);
                var z = BitString.MSBSubstring(Sha.HashMessage(message).Digest, 0, zLen).ToPositiveBigInteger();

                s = (kInv * (z + x * r)) % q;

            } while (r == 0 || s == 0);

            return new FfcSignatureResult(new FfcSignature(new BitString(r), new BitString(s)));
        }

        public FfcVerificationResult Verify(FfcDomainParameters domainParameters, FfcKeyPair keyPair, BitString message, FfcSignature signature, bool skipHash = false)
        {
            var y = keyPair.PublicKeyY.ToPositiveBigInteger();
            var p = domainParameters.P.ToPositiveBigInteger();
            var q = domainParameters.Q.ToPositiveBigInteger();
            var g = domainParameters.G.ToPositiveBigInteger();
            var r = signature.R.ToPositiveBigInteger();
            var s = signature.S.ToPositiveBigInteger();
            
            // 1
            if(r < 0 || r > q)
            {
                return new FfcVerificationResult("Invalid r provided");
            }

            if(s < 0 || s > q)
            {
                return new FfcVerificationResult("Invalid s provided");
            }

            // 2
            var w = s.ModularInverse(q);
            var zLen = System.Math.Min(Sha.HashFunction.OutputLen, new BitString(q).BitLength);
            var z = BitString.MSBSubstring(Sha.HashMessage(message).Digest, 0, zLen).ToPositiveBigInteger();
            var u1 = (z * w) % q;
            var u2 = (r * w) % q;

            // (g^u1 * y^u2) mod p == [(g^u1 mod p) * (y^u2 mod p)] mod p
            var v = ((BigInteger.ModPow(g, u1, p) * BigInteger.ModPow(y, u2, p)) % p) % q;

            // 3
            if(v != r)
            {
                return new FfcVerificationResult("Invalid v, does not match provided r");
            }

            return new FfcVerificationResult();
        }
    }
}
