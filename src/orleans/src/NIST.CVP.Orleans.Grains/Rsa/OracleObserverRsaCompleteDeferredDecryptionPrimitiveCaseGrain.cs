using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.Orleans.Grains.Rsa
{
    public class OracleObserverRsaCompleteDeferredDecryptionPrimitiveCaseGrain : ObservableOracleGrainBase<RsaDecryptionPrimitiveResult>, 
        IOracleObserverRsaCompleteDeferredDecryptionPrimitiveCaseGrain
    {
        private readonly IRsa _rsa;
        
        private RsaDecryptionPrimitiveParameters _param;
        private RsaDecryptionPrimitiveResult _fullParam;

        public OracleObserverRsaCompleteDeferredDecryptionPrimitiveCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IRsa rsa
        ) : base (nonOrleansScheduler)
        {
            _rsa = rsa;
        }
        
        public async Task<bool> BeginWorkAsync(RsaDecryptionPrimitiveParameters param, RsaDecryptionPrimitiveResult fullParam)
        {
            _param = param;
            _fullParam = fullParam;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var rsaResult = _rsa.Encrypt(_fullParam.PlainText.ToPositiveBigInteger(), _fullParam.Key.PubKey);
            
            RsaDecryptionPrimitiveResult result = null;
            if (rsaResult.Success)
            {
                result = new RsaDecryptionPrimitiveResult
                {
                    CipherText = new BitString(rsaResult.CipherText, _param.Modulo, false),
                    TestPassed = true
                };
            }
            else
            {
                result = new RsaDecryptionPrimitiveResult
                {
                    TestPassed = false
                };
            }

            // Notify observers of result
            await Notify(result);
        }
    }
}