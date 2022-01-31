using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Tdes;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Tdes
{
    public class OracleObserverTdesMctCaseGrain : ObservableOracleGrainBase<MctResult<TdesResult>>,
        IOracleObserverTdesMctCaseGrain
    {
        private readonly IMonteCarloFactoryTdes _mctFactory;
        private readonly IEntropyProvider _entropyProvider;

        private TdesParameters _param;

        public OracleObserverTdesMctCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IMonteCarloFactoryTdes mctFactory,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
        {
            _mctFactory = mctFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public async Task<bool> BeginWorkAsync(TdesParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var cipher = _mctFactory.GetInstance(_param.Mode);
            var direction = BlockCipherDirections.Encrypt;
            if (_param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _entropyProvider.GetEntropy(_param.DataLength);
            var key = TdesHelpers.GenerateTdesKey(_param.KeyingOption);
            var iv = _entropyProvider.GetEntropy(64);

            var blockCipherParams = new ModeBlockCipherParameters(
                direction,
                iv.GetDeepCopy(),
                key.GetDeepCopy(),
                payload.GetDeepCopy()
            );
            var result = cipher.ProcessMonteCarloTest(blockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new MctResult<TdesResult>
            {
                Seed = new TdesResult()
                {
                    Key = key,
                    Iv = iv,
                    PlainText = direction == BlockCipherDirections.Encrypt ? payload : null,
                    CipherText = direction == BlockCipherDirections.Decrypt ? payload : null,
                },
                Results = Array.ConvertAll(result.Response.ToArray(), element => new TdesResult
                {
                    Key = element.Keys,
                    Iv = element.IV,
                    PlainText = element.PlainText,
                    CipherText = element.CipherText
                }).ToList()
            });
        }
    }
}
