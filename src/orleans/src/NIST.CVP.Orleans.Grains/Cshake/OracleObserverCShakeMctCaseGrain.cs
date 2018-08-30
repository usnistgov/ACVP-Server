using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Cshake;

namespace NIST.CVP.Orleans.Grains.Cshake
{
    public class OracleObserverCShakeMctCaseGrain : ObservableOracleGrainBase<MctResult<CShakeResult>>, 
        IOracleObserverCShakeMctCaseGrain
    {
        private readonly ICSHAKE_MCT _cshakeMct;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IRandom800_90 _rand;

        private CShakeParameters _param;

        public OracleObserverCShakeMctCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ICSHAKE_MCT cshakeMct,
            IEntropyProviderFactory entropyProviderFactory,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _cshakeMct = cshakeMct;
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
            _rand = rand;
        }
        
        public async Task<bool> BeginWorkAsync(CShakeParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var message = _rand.GetRandomBitString(_param.MessageLength);

            // TODO isSample up in here?
            var result = _cshakeMct.MCTHash(_param.HashFunction, message, _param.OutLens, true); // currently always a sample

            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new MctResult<CShakeResult>
            {
                Results = result.Response.ConvertAll(element =>
                    new CShakeResult { Message = element.Message, Digest = element.Digest, Customization = element.Customization })
            });
        }
    }
}