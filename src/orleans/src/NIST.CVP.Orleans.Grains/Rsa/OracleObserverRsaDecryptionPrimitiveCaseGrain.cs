﻿using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Math;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Rsa;
using System.Threading.Tasks;

namespace NIST.CVP.Orleans.Grains.Rsa
{
    public class OracleObserverRsaDecryptionPrimitiveCaseGrain : ObservableOracleGrainBase<RsaDecryptionPrimitiveResult>, 
        IOracleObserverRsaDecryptionPrimitiveCaseGrain
    {
        private readonly IRsa _rsa;
        private readonly IRandom800_90 _rand;

        private RsaDecryptionPrimitiveParameters _param;
        private KeyResult _passingKey;

        public OracleObserverRsaDecryptionPrimitiveCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IRsa rsa,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
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
            RsaDecryptionPrimitiveResult result;

            // TODO use an additional parameter for this grain, passing in a key to be able to make use of pools.
            if (_param.TestPassed)
            {
                var cipherText = new BitString(_rand.GetRandomBigInteger(1, _passingKey.Key.PubKey.N - 1));
                var plainText = _rsa.Decrypt(cipherText.ToPositiveBigInteger(), _passingKey.Key.PrivKey, _passingKey.Key.PubKey).PlainText;

                result = new RsaDecryptionPrimitiveResult
                {
                    CipherText = cipherText,
                    Key = _passingKey.Key,
                    PlainText = new BitString(plainText, _param.Modulo, false)
                };
            }
            else
            {
                // Failure tests - save some time and generate a dummy key

                // Pick a random ciphertext and force a leading '1' (so that it MUST be 2048 bits)
                var cipherText = BitString.One().ConcatenateBits(_rand.GetRandomBitString(_param.Modulo - 1));

                // Pick a random n that is 2048 bits and less than the ciphertext
                var n = _rand.GetRandomBigInteger(NumberTheory.Pow2(_param.Modulo - 1), cipherText.ToPositiveBigInteger());
                var e = KeyGenHelper.GetEValue(RsaRunner.RSA_PUBLIC_EXPONENT_BITS_MIN, RsaRunner.RSA_PUBLIC_EXPONENT_BITS_MAX).ToPositiveBigInteger();

                result = new RsaDecryptionPrimitiveResult
                {
                    CipherText = cipherText,
                    Key = new KeyPair { PubKey = new PublicKey { E = e, N = n } }
                };
            }

            // Notify observers of result
            await Notify(result);
        }
    }
}