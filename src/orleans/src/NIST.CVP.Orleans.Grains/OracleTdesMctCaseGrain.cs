using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Enums;

namespace NIST.CVP.Orleans.Grains
{
    
    public class OracleTdesMctCaseGrain : PollableOracleGrainBase<MctResult<TdesResult>>, IOracleTdesMctCaseGrain
    {
        private readonly IMonteCarloFactoryTdes _mctFactory;
        private readonly IEntropyProvider _entropyProvider;

        private TdesParameters _param;

        public OracleTdesMctCaseGrain(
            LimitedConcurrencyLevelTaskScheduler scheduler
            //IMonteCarloFactoryTdes mctFactory,
            //IEntropyProviderFactory entropyProviderFactory
        )
            : base(scheduler)
        {
            //_mctFactory = mctFactory;
            //_entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);

            _mctFactory = new TdesMonteCarloFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory());
            _entropyProvider = new EntropyProviderFactory().GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public async Task<bool> BeginWorkAsync(TdesParameters param)
        {
            _param = param;
            return await BeginGrainWorkAsync();
        }

        protected override Task DoWorkAsync()
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

            var blockCipherParams = new ModeBlockCipherParameters(direction, iv, key, payload);
            var result = cipher.ProcessMonteCarloTest(blockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            Result = new MctResult<TdesResult>
            {
                Results = Array.ConvertAll(result.Response.ToArray(), element => new TdesResult
                {
                    Key = element.Keys,
                    Iv = element.IV,
                    PlainText = element.PlainText,
                    CipherText = element.CipherText
                }).ToList()
            };

            State = GrainState.CompletedWork;
            return Task.CompletedTask;
        }
    }
}