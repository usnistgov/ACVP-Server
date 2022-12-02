using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Math;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Rsa;


namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Rsa
{
    public class OracleObserverRsaDpSp800Br2CaseGrain : ObservableOracleGrainBase<RsaDecryptionPrimitiveResult>,
        IOracleObserverRsaCompleteDpSp800Br2CaseGrain
    {
        private readonly IRsa _rsa;
        private readonly IRandom800_90 _rand;

        private RsaDecryptionPrimitiveParameters _param;
        private KeyResult _passingKey;

        public OracleObserverRsaDpSp800Br2CaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IRsa rsa,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)   
        {
            _rsa = rsa;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(RsaDecryptionPrimitiveParameters param, KeyResult passingKey)
        {
            _param = param;
            _passingKey = passingKey;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var cipherText = new BitString(_rand.GetRandomBigInteger(1, _passingKey.Key.PubKey.N - 1));
            var plainText = _rsa.Decrypt(cipherText.ToPositiveBigInteger(), _passingKey.Key.PrivKey, _passingKey.Key.PubKey).PlainText;
            
            // Store everything and notify observers of result
            await Notify(
                new RsaDecryptionPrimitiveResult
                {
                    CipherText = cipherText,
                    Key = _passingKey.Key,
                    PlainText = new BitString(plainText, _param.Modulo, false),
                    TestPassed = true
                }
            );
        }
    }
}
