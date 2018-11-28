using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.Orleans.Grains.Rsa
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
        ) : base (nonOrleansScheduler)
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