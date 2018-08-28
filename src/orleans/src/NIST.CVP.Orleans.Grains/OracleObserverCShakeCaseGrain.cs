using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces;

namespace NIST.CVP.Orleans.Grains
{
    public class OracleObserverCShakeCaseGrain : ObservableOracleGrainBase<CShakeResult>, 
        IOracleObserverCShakeCaseGrain
    {
        private readonly ICSHAKE _cshake;
        private readonly IEntropyProvider _entropyProvider;
        private readonly IRandom800_90 _rand;

        private CShakeParameters _param;

        public OracleObserverCShakeCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ICSHAKE cshake,
            IEntropyProviderFactory entropyProviderFactory,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _cshake = cshake;
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
            Crypto.Common.Hash.HashResult result = null;
            var message = _entropyProvider.GetEntropy(_param.MessageLength);

            BitString customizationHex = null;
            string customization = "";
            if (_param.HexCustomization)
            {
                customizationHex = _entropyProvider.GetEntropy(_param.CustomizationLength);
                result = _cshake.HashMessage(_param.HashFunction, message, customizationHex, _param.FunctionName);
            }
            else
            {
                customization = _rand.GetRandomString(_param.CustomizationLength);
                result = _cshake.HashMessage(_param.HashFunction, message, customization, _param.FunctionName);
            }

            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new CShakeResult
            {
                Message = message,
                Digest = result.Digest,
                Customization = customization,
                CustomizationHex = customizationHex,
                FunctionName = _param.FunctionName
            });
        }
    }
}