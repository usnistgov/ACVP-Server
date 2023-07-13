using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Hash;
using HashResult = NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.HashResult;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Hash
{
    public class OracleObserverShaMctCaseGrain : ObservableOracleGrainBase<Oracle.Abstractions.ResultTypes.MctResult<HashResult>>,
        IOracleObserverShaMctCaseGrain
    {
        private readonly IRandom800_90 _rand = new Random800_90();
        private readonly IShaFactory _shaFactory;
        private readonly IEntropyProvider _entropyProvider;
        
        private static int MIN_MESSAGE_LENGTH = 32; // 0 is supported, but for MCT 1 is the min
        private static int MAX_MESSAGE_LENGTH = 65536;
        
        private ShaParameters _param;

        public OracleObserverShaMctCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IShaFactory shaFactory,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
        {
            _shaFactory = shaFactory;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public async Task<bool> BeginWorkAsync(ShaParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var shaMct = _shaFactory.GetShaMctInstance(_param.HashFunction, _param.IsAlternate);
            var seed = _entropyProvider.GetEntropy(_param.MessageLength);
            
            var result = shaMct.MctHash(seed);

            if (!result.Success)
            {
                throw new Exception();
            }

            await Notify(new Oracle.Abstractions.ResultTypes.MctResult<HashResult>
            {
                Seed = new HashResult() { Message = seed },
                Results = result.Response.ConvertAll(element =>
                    new HashResult { Message = element.Message, Digest = element.Digest })
            });
        }
    }
}
