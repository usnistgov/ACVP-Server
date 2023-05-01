using System;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Math;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
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
            BitString cipherText = new BitString(0);
            BitString plainText = null;
            var reason = EnumHelpers.GetEnumFromEnumDescription<RsaDpDisposition>(_param.Disposition);
            
            switch (reason)
            {
                case RsaDpDisposition.None:
                    cipherText = new BitString(_rand.GetRandomBigInteger(2, _passingKey.Key.PubKey.N - 2), _param.Modulo);
                    plainText = new BitString(_rsa.Decrypt(cipherText.ToPositiveBigInteger(), _passingKey.Key.PrivKey, 
                        _passingKey.Key.PubKey).PlainText, _param.Modulo);
                    break;
                // CipherText = 0
                case RsaDpDisposition.CtEqual0:
                    cipherText = new BitString(bigInt:0, _param.Modulo);
                    break;
                // CipherText = 1
                case RsaDpDisposition.CtEqual1:
                    cipherText = new BitString(bigInt: 1, _param.Modulo);
                    break;
                // CipherText = N - 1
                case RsaDpDisposition.CtEqualNMinusOne:
                    cipherText = new BitString(_passingKey.Key.PubKey.N - 1, _param.Modulo);
                    break;
                // CipherText > N - 1
                case RsaDpDisposition.CtGreaterNMinusOne:
                    cipherText = new BitString(_rand.GetRandomBigInteger(_passingKey.Key.PubKey.N, NumberTheory.Pow2(_param.Modulo)), _param.Modulo);
                    break;
            }
            
            // Store everything and notify observers of result
            await Notify(
                new RsaDecryptionPrimitiveResult
                {
                    CipherText = cipherText,
                    Key = _passingKey.Key,
                    PlainText = plainText,
                    TestPassed = reason == RsaDpDisposition.None
                }
            );
        }
    }
}
