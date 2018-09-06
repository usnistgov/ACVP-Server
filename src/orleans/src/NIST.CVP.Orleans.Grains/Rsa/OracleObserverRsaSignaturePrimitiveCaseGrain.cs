using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.Orleans.Grains.Rsa
{
    public class OracleObserverRsaSignaturePrimitiveCaseGrain : ObservableOracleGrainBase<RsaSignaturePrimitiveResult>, 
        IOracleObserverRsaSignaturePrimitiveCaseGrain
    {
        private readonly IRsa _rsa;
        private readonly IRsaRunner _rsaRunner;
        private readonly IRandom800_90 _rand;

        private RsaSignaturePrimitiveParameters _param;

        public OracleObserverRsaSignaturePrimitiveCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IRsa rsa,
            IRsaRunner rsaRunner,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _rsa = rsa;
            _rsaRunner = rsaRunner;
            _rand = rand;
        }
        
        public async Task<bool> BeginWorkAsync(RsaSignaturePrimitiveParameters param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var keyParam = new RsaKeyParameters
            {
                KeyFormat = _param.KeyFormat,
                Modulus = _param.Modulo,
                PrimeTest = PrimeTestModes.C2,
                PublicExponentMode = PublicExponentModes.Random,
                KeyMode = PrimeGenModes.B33
            };

            var key = _rsaRunner.GetRsaKey(keyParam).Key;

            var shouldPass = _rand.GetRandomInt(0, 2) == 0;
            BitString message;
            BitString signature = null;
            if (shouldPass)
            {
                // No failure, get a random 2048-bit value less than N
                message = new BitString(_rand.GetRandomBigInteger(key.PubKey.N), 2048);
                signature = new BitString(_rsa.Decrypt(message.ToPositiveBigInteger(), key.PrivKey, key.PubKey).PlainText, 2048);
            }
            else
            {
                // Yes failure, get a random 2048-bit value greater than N
                message = new BitString(_rand.GetRandomBigInteger(key.PubKey.N, NumberTheory.Pow2(2048)), 2048);
            }

            // Notify observers of result
            await Notify(new RsaSignaturePrimitiveResult
            {
                Key = key,
                Message = message,
                Signature = signature,
                ShouldPass = shouldPass
            });
        }
    }
}