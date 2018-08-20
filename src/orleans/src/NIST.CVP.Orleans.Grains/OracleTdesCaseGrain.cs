using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Enums;

namespace NIST.CVP.Orleans.Grains
{
    public class OracleTdesCaseGrain : PollableOracleGrainBase<TdesResult>, IOracleTdesCaseGrain<TdesResult>
    {
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IModeBlockCipherFactory _modeFactory;
        private readonly IEntropyProvider _entropyProvider;

        private TdesParameters _param;

        public OracleTdesCaseGrain(
            LimitedConcurrencyLevelTaskScheduler scheduler,
            IBlockCipherEngineFactory engineFactory,
            IModeBlockCipherFactory modeFactory,
            IEntropyProviderFactory entropyProviderFactory
        )
            : base(scheduler)
        {
            _engineFactory = engineFactory;
            _modeFactory = modeFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public async Task<bool> BeginWorkAsync(TdesParameters param)
        {
            _param = param;
            return await BeginGrainWorkAsync();
        }

        protected override Task DoWorkAsync()
        {
            var cipher = _modeFactory.GetStandardCipher(
                _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes),
                _param.Mode
            );
            var direction = BlockCipherDirections.Encrypt;
            if (_param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _entropyProvider.GetEntropy(_param.DataLength);
            var key = TdesHelpers.GenerateTdesKey(_param.KeyingOption);
            var iv = _entropyProvider.GetEntropy(64);

            var blockCipherParams = new ModeBlockCipherParameters(direction, iv, key, payload);
            var result = cipher.ProcessPayload(blockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            Result = new TdesResult
            {
                PlainText = direction == BlockCipherDirections.Encrypt ? payload : result.Result,
                CipherText = direction == BlockCipherDirections.Decrypt ? payload : result.Result,
                Key = key,
                Iv = iv
            };

            State = GrainState.CompletedWork;
            return Task.CompletedTask;
        }
    }
}