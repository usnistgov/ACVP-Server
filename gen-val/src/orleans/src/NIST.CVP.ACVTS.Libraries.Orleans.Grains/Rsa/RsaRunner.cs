using Microsoft.Extensions.Logging;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Rsa
{
    public class RsaRunner : IRsaRunner
    {
        public const int RSA_PUBLIC_EXPONENT_BITS_MIN = 32;
        public const int RSA_PUBLIC_EXPONENT_BITS_MAX = 64;
        private readonly ILogger<RsaRunner> _logger;
        private readonly IShaFactory _shaFactory;
        private readonly IKeyComposerFactory _keyComposerFactory;
        private readonly IKeyBuilder _keyBuilder;

        public RsaRunner(
            ILogger<RsaRunner> logger,
            IShaFactory shaFactory,
            IKeyComposerFactory keyComposerFactory,
            IKeyBuilder keyBuilder
        )
        {
            _logger = logger;
            _shaFactory = shaFactory;
            _keyComposerFactory = keyComposerFactory;
            _keyBuilder = keyBuilder;
        }

        public RsaPrimeResult GeneratePrimes(RsaKeyParameters param, IEntropyProvider entropyProvider)
        {
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
                .WithPMod8(param.PMod8)
                .WithQMod8(param.QMod8)
                .Build();

            if (!keyResult.Success)
            {
                _logger.LogWarning(keyResult.ErrorMessage);
                return new RsaPrimeResult
                {
                    Success = false,
                    ErrorMessage = keyResult.ErrorMessage
                };
                // throw new RsaPrimeGenException(keyResult.ErrorMessage);
            }

            return new RsaPrimeResult
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
    }
}
