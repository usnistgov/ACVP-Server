using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Aead;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Aead;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Aead
{
    public class OracleObserverAesCcmCaseGrain : ObservableOracleGrainBase<AeadResult>,
        IOracleObserverAesCcmCaseGrain
    {
        private const double FAIL_RATE = 0.25;
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IAeadRunner _aeadRunner;
        private readonly IAeadModeBlockCipherFactory _aeadCipherFactory;
        private readonly IRandom800_90 _rand;

        private AeadParameters _param;

        public OracleObserverAesCcmCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IBlockCipherEngineFactory engineFactory,
            IEntropyProviderFactory entropyProviderFactory,
            IAeadRunner aeadRunner,
            IAeadModeBlockCipherFactory aeadCipherFactory,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _engineFactory = engineFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
            _aeadRunner = aeadRunner;
            _aeadCipherFactory = aeadCipherFactory;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(AeadParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var fullParams = new AeadResult
            {
                PlainText = _entropyProvider.GetEntropy(_param.PayloadLength),
                Key = _entropyProvider.GetEntropy(_param.KeyLength),
                Iv = _entropyProvider.GetEntropy(_param.IvLength),
                Aad = _entropyProvider.GetEntropy(_param.AadLength),
            };

            var result = _aeadRunner.DoSimpleAead(
                _aeadCipherFactory.GetAeadCipher(
                    _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes),
                    BlockCipherModesOfOperation.Ccm
                ),
                fullParams, _param
            );

            if (_param.CouldFail)
            {
                // Should Fail at certain ratio, 25%
                var upperBound = (int)(1.0 / FAIL_RATE);
                var shouldFail = _rand.GetRandomInt(0, upperBound) == 0;

                if (shouldFail)
                {
                    var ctPortion = _param.PayloadLength == 0
                        ? new BitString(0)
                        : result.CipherText.GetMostSignificantBits(result.CipherText.BitLength - _param.TagLength);
                    // Change the tag portion of the ciphertext
                    var tagPortion =
                        _rand.GetDifferentBitStringOfSameSize(
                            result.CipherText.GetLeastSignificantBits(_param.TagLength));

                    result.CipherText = ctPortion.ConcatenateBits(tagPortion);
                    result.TestPassed = false;
                }
            }

            // Notify observers of result
            await Notify(result);
        }
    }
}
