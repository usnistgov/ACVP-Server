using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Rsa
{
    public class OracleObserverRsaDeferredSignatureCaseGrain : ObservableOracleGrainBase<RsaSignatureResult>,
        IOracleObserverRsaDeferredSignatureCaseGrain
    {
        private readonly IRandom800_90 _rand;

        private RsaSignatureParameters _param;

        public OracleObserverRsaDeferredSignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(RsaSignatureParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var messageLength = _param.MessageLength == 0 ? _param.Modulo / 2 : _param.MessageLength;

            // Notify observers of result
            await Notify(new RsaSignatureResult
            {
                Message = _rand.GetRandomBitString(messageLength)
            });
        }
    }
}
