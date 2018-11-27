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

        private readonly IShaFactory _shaFactory;
        private readonly IKeyComposerFactory _keyComposerFactory;
        private readonly IKeyBuilder _keyBuilder;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IKeyGenParameterHelper _keyGenHelper;
        
        public RsaRunner(
            IShaFactory shaFactory,
            IKeyComposerFactory keyComposerFactory,
            IKeyBuilder keyBuilder,
            IEntropyProviderFactory entropyProviderFactory,
            IKeyGenParameterHelper keyGenHelper
        )
        {
            _shaFactory = shaFactory;
            _keyComposerFactory = keyComposerFactory;
            _keyBuilder = keyBuilder;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
            _keyGenHelper = keyGenHelper;
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
                .Build();

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
        
        
        public RsaKeyResult GetRsaKey(RsaKeyParameters param)
        {
            RsaPrimeResult result = null;
            do
            {
                param.Seed = _keyGenHelper.GetSeed(param.Modulus);
                param.PublicExponent = param.PublicExponentMode == PublicExponentModes.Fixed ? 
                    param.PublicExponent : 
                    _keyGenHelper.GetEValue(RSA_PUBLIC_EXPONENT_BITS_MIN, RSA_PUBLIC_EXPONENT_BITS_MAX);
                param.BitLens = _keyGenHelper.GetBitlens(param.Modulus, param.KeyMode);
                
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