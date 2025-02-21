using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Eddsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Eddsa
{
    public class OracleObserverEddsaDeferredSignatureCaseGrain : ObservableOracleGrainBase<EddsaSignatureResult>,
        IOracleObserverEddsaDeferredSignatureCaseGrain
    {
        private readonly IRandom800_90 _rand;       

        private EddsaSignatureParameters _param;
        
        private const int BITS_IN_BYTE = 8;

        public OracleObserverEddsaDeferredSignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEntropyProviderFactory entropyProviderFactory,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(EddsaSignatureParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var message = _rand.GetRandomBitString(1024);

            // _param.ContextLength will either have been 1) explicitly set to a value or 2) not set to a value. In the 
            // case of #2, _param.ContextLength will default to 0 -- returns "new BitString("")" if input is <= 0
            var context = _rand.GetRandomBitString(_param.ContextLength * BITS_IN_BYTE);
            
            // Notify observers of result
            await Notify(new EddsaSignatureResult
            {
                Message = message,
                Context = context,
                ContextLength = context.BitLength/BITS_IN_BYTE
            });
        }
    }
}
