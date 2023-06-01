using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Math;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Rsa
{
    public class OracleObserverRsaSignaturePrimitiveV2_0CaseGrain : ObservableOracleGrainBase<RsaSignaturePrimitiveResult>,
        IOracleObserverRsaSignaturePrimitiveV2_0CaseGrain
    {
        private readonly IRsa _rsa;
        private readonly IRandom800_90 _rand;

        private RsaSignaturePrimitiveParameters _param;
        private RsaKeyResult _key;

        public OracleObserverRsaSignaturePrimitiveV2_0CaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IRsa rsa,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _rsa = rsa;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(RsaSignaturePrimitiveParameters param, RsaKeyResult key)
        {
            _param = param;
            _key = key;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            BitString message = new BitString(0);
            BitString signature = null;
            var reason = EnumHelpers.GetEnumFromEnumDescription<RsaSpDisposition>(_param.Disposition);
            
            switch (reason)
            {
                case RsaSpDisposition.None:
                    message = new BitString(_rand.GetRandomBigInteger(_key.Key.PubKey.N-1), _param.Modulo);
                    signature = new BitString(_rsa.Decrypt(message.ToPositiveBigInteger(), _key.Key.PrivKey, _key.Key.PubKey).PlainText, _param.Modulo);
                    break;
                // message = N
                case RsaSpDisposition.MsgEqualN:
                    message = new BitString(_key.Key.PubKey.N, _param.Modulo);
                    break;
                // message > N but < modulo all 1's
                case RsaSpDisposition.MsgGreaterNLessModulo:
                    message = new BitString(_rand.GetRandomBigInteger(_key.Key.PubKey.N+1, NumberTheory.Pow2(_param.Modulo) ), _param.Modulo);
                    break;
            }
            
            await Notify(new RsaSignaturePrimitiveResult
            {
                Key = _key.Key,
                Message = message,
                Signature = signature,
                ShouldPass = reason == RsaSpDisposition.None
            });
        }
    }
}
