using Microsoft.Extensions.Logging;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Orleans.Grains.Rsa
{
    public class RsaRunner : IRsaRunner
    {
        public const int RSA_PUBLIC_EXPONENT_BITS_MIN = 32;
        public const int RSA_PUBLIC_EXPONENT_BITS_MAX = 64;
        private readonly ILogger<RsaRunner> _logger;
        private readonly IShaFactory _shaFactory;
        private readonly IKeyComposerFactory _keyComposerFactory;
        private readonly IKeyBuilder _keyBuilder;
        private readonly IEntropyProvider _entropyProvider;
        
        public RsaRunner(
            ILogger<RsaRunner> logger,
            IShaFactory shaFactory,
            IKeyComposerFactory keyComposerFactory,
            IKeyBuilder keyBuilder,
            IEntropyProviderFactory entropyProviderFactory
        )
        {
            _logger = logger;
            _shaFactory = shaFactory;
            _keyComposerFactory = keyComposerFactory;
            _keyBuilder = keyBuilder;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public RsaPrimeResult GeneratePrimes(RsaKeyParameters param, IEntropyProvider entropyProvider)
        {
            // TODO Not every group has a hash alg... Can use a default value perhaps?
            ISha sha = null;
            if (param.HashAlg != null)
            {
                sha = _shaFactory.GetShaInstance(param.HashAlg);
            }

            var keyComposer = _keyComposerFactory.GetKeyComposer(param.KeyFormat);

            // Configure Prime Generator
            var keyResult = _keyBuilder
                .WithBitlens(param.BitLens)
                .WithEntropyProvider(entropyProvider)
                .WithHashFunction(sha)
                .WithNlen(param.Modulus)
                .WithPrimeGenMode(param.KeyMode)
                .WithPrimeTestMode(param.PrimeTest)
                .WithPublicExponent(param.PublicExponent)
                .WithKeyComposer(keyComposer)
                .WithSeed(param.Seed)
                .WithStandard(param.Standard)
                .Build();

            if (!keyResult.Success)
            {
                _logger.LogDebug(keyResult.ErrorMessage);
            }

            return new RsaPrimeResult()
            {
                Success = keyResult.Success,
                Key = keyResult.Key,
                Aux = keyResult.AuxValues
            };
        }

        public RsaKeyResult CompleteKey(RsaKeyResult param, PrivateKeyModes keyMode)
        {
            var keyComposer = _keyComposerFactory.GetKeyComposer(keyMode);
            var primePair = new PrimePair
            {
                P = param.Key.PrivKey.P,
                Q = param.Key.PrivKey.Q
            };

            return new RsaKeyResult
            {
                Key = keyComposer.ComposeKey(param.Key.PubKey.E, primePair)
            };
        }

        // TODO is this used?
        public RsaKeyResult GetRsaKey(RsaKeyParameters param)
        {
            RsaPrimeResult result;
            do
            {
                param.Seed = KeyGenHelper.GetSeed(param.Modulus);
                param.PublicExponent = param.PublicExponentMode == PublicExponentModes.Fixed ? 
                    param.PublicExponent : 
                    KeyGenHelper.GetEValue(RSA_PUBLIC_EXPONENT_BITS_MIN, RSA_PUBLIC_EXPONENT_BITS_MAX);
                param.BitLens = KeyGenHelper.GetBitlens(param.Modulus, param.KeyMode);
                
                // Generate key until success
                result = GeneratePrimes(param, _entropyProvider);

            } while (!result.Success);

            return new RsaKeyResult
            {
                Key = result.Key,
                AuxValues = result.Aux,
                BitLens = param.BitLens,
                Seed = param.Seed
            };
        }
    }
}