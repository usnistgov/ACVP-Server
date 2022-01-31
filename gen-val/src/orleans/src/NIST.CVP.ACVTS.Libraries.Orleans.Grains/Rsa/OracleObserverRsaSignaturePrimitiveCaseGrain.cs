using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Math;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Rsa
{
    public class OracleObserverRsaSignaturePrimitiveCaseGrain : ObservableOracleGrainBase<RsaSignaturePrimitiveResult>,
        IOracleObserverRsaSignaturePrimitiveCaseGrain
    {
        private readonly IRsa _rsa;
        private readonly IRandom800_90 _rand;

        private RsaSignaturePrimitiveParameters _param;
        private RsaKeyResult _key;

        public OracleObserverRsaSignaturePrimitiveCaseGrain(
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
            var shouldPass = _rand.GetRandomInt(0, 2) == 0;
            BitString message;
            BitString signature = null;
            if (shouldPass)
            {
                // No failure, get a random 2048-bit value less than N
                message = new BitString(_rand.GetRandomBigInteger(_key.Key.PubKey.N), 2048);
                signature = new BitString(_rsa.Decrypt(message.ToPositiveBigInteger(), _key.Key.PrivKey, _key.Key.PubKey).PlainText, 2048);
            }
            else
            {
                // Yes failure, get a random 2048-bit value greater than N
                message = new BitString(_rand.GetRandomBigInteger(_key.Key.PubKey.N, NumberTheory.Pow2(2048)), 2048);
            }

            // Notify observers of result
            await Notify(new RsaSignaturePrimitiveResult
            {
                Key = _key.Key,
                Message = message,
                Signature = signature,
                ShouldPass = shouldPass
            });
        }
    }
}
