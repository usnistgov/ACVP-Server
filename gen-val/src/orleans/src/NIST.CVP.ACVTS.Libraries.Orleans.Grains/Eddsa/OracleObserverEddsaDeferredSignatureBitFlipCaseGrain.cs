using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Eddsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Eddsa
{
    public class OracleObserverEddsaDeferredSignatureBitFlipCaseGrain : ObservableOracleGrainBase<EddsaSignatureResult>,
        IOracleObserverEddsaDeferredSignatureBitFlipCaseGrain
    {
        private EddsaSignatureParameters _param;
        private const int BITS_IN_BYTE = 8;
        
        public OracleObserverEddsaDeferredSignatureBitFlipCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler
        ) : base(nonOrleansScheduler)
        {

        }

        public async Task<bool> BeginWorkAsync(EddsaSignatureParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var message = _param.Message;
            
            if (_param.Bit != -1)
            {
                message.Bits.Set(_param.Bit, !message.Bits.Get(_param.Bit));      // flip bit
            }

            // Notify observers of result
            await Notify(new EddsaSignatureResult
            {
                Message = message,
                Context = _param.Context,
                ContextLength = _param.ContextLength/BITS_IN_BYTE
            });
        }
    }
}
