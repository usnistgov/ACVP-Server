using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Enums;
using Orleans;
using Orleans.Providers;

namespace NIST.CVP.Orleans.Grains
{
    [StorageProvider(ProviderName = Constants.StorageProviderName)]
    public class OracleMctResultTdesGrain : Grain<GrainState>, IOracleMctResultTdesGrain
    {
        private readonly TdesMonteCarloFactory _tdesMctFactory = new TdesMonteCarloFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory());
        private readonly Random800_90 _rand = new Random800_90();

        private MctResult<TdesResult> _result;

        public async Task<MctResult<TdesResult>> GetTdesMctCaseAsync(TdesParameters param)
        {
            switch (State)
            {
                case GrainState.Faulted:
                    throw new NotSupportedException(
                        $"{this} is in state {State} and not available for further invocations."
                    );
                case GrainState.Initialized:
                    State = GrainState.Working;
                    await WriteStateAsync();

                    Task.Run(() =>
                    {
                        PerformWorkAsync(param).FireAndForget();
                    }).FireAndForget();

                    return (null);
                case GrainState.Working:
                    return (null);
                case GrainState.CompletedWork:
                    State = GrainState.ShouldDispose;
                    await WriteStateAsync();
                    
                    return (_result);
                default:
                    throw new ArgumentException($"Unexpected {nameof(State)}");
            }
        }

        private async Task PerformWorkAsync(TdesParameters param)
        {
            var cipher = _tdesMctFactory.GetInstance(param.Mode);
            var direction = BlockCipherDirections.Encrypt;
            if (param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _rand.GetRandomBitString(param.DataLength);
            var key = TdesHelpers.GenerateTdesKey(param.KeyingOption);
            var iv = _rand.GetRandomBitString(64);

            var blockCipherParams = new ModeBlockCipherParameters(direction, iv, key, payload);
            var result = cipher.ProcessMonteCarloTest(blockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            _result = new MctResult<TdesResult>
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
            await WriteStateAsync();
        }
    }
}