using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.SafePrimes;

namespace NIST.CVP.Orleans.Grains.SafePrimes
{
    public class ObserverSafePrimesKeyVerGrain : ObservableOracleGrainBase<SafePrimesKeyVerResult>,
        IObserverSafePrimesKeyVerGrain
    {
        private readonly IDsaFfcFactory _dsaFactory;
        private readonly IRandom800_90 _random;
        
        private SafePrimesKeyVerParameters _param;
        
        public ObserverSafePrimesKeyVerGrain(LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IDsaFfcFactory dsaFactory,
            IRandom800_90 random) 
            : base(nonOrleansScheduler)
        {
            _dsaFactory = dsaFactory;
            _random = random;
        }

        public async Task<bool> BeginWorkAsync(SafePrimesKeyVerParameters param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var key = _dsaFactory
                .GetInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256))
                .GenerateKeyPair(_param.DomainParameters);

            var result = new SafePrimesKeyVerResult()
            {
                Disposition = SafePrimesKeyDisposition.Valid,
                TestPassed = true,
                KeyPair = key.KeyPair
            };
            
            if (_param.Disposition == SafePrimesKeyDisposition.Invalid)
            {
                var publicKey = new BitString(key.KeyPair.PublicKeyY);
                var publicKeyBytesLen = publicKey.BitLength.CeilingDivide(BitString.BITSINBYTE);
                
                // modify a random byte within the key
                publicKey[_random.GetRandomInt(0, publicKeyBytesLen)] += 2;

                result.KeyPair.PublicKeyY = publicKey.ToPositiveBigInteger();
                result.Disposition = SafePrimesKeyDisposition.Invalid;
                result.TestPassed = false;
            }

            await Notify(result);
        }
    }
}