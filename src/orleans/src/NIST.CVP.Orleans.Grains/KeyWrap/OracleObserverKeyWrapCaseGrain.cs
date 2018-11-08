﻿using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.MAC.CMAC;
using NIST.CVP.Crypto.Common.Symmetric.KeyWrap;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.KeyWrap;

namespace NIST.CVP.Orleans.Grains.KeyWrap
{
    public class OracleObserverKeyWrapCaseGrain : ObservableOracleGrainBase<KeyWrapResult>, 
        IOracleObserverKeyWrapCaseGrain
    {
        private const double FAIL_RATIO = 0.2;

        private readonly IKeyWrapFactory _keyWrapFactory;
        private readonly IRandom800_90 _rand;

        private KeyWrapParameters _param;

        public OracleObserverKeyWrapCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKeyWrapFactory keyWrapFactory,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _keyWrapFactory = keyWrapFactory;
            _rand = rand;
        }
        
        public async Task<bool> BeginWorkAsync(KeyWrapParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var keyWrap = _keyWrapFactory.GetKeyWrapInstance(_param.KeyWrapType);

            var key = _rand.GetRandomBitString(_param.KeyLength);
            var payload = _rand.GetRandomBitString(_param.DataLength);

            var result = new KeyWrapResult()
            {
                Key = key,
                Plaintext = payload,
                Ciphertext = keyWrap.Encrypt(key, payload, _param.WithInverseCipher).Result,
                TestPassed = true
            };

            if (_param.CouldFail)
            {
                // Should Fail at certain ratio, 20%
                var upperBound = (int)(1.0 / FAIL_RATIO);
                var shouldFail = _rand.GetRandomInt(0, upperBound) == 0;

                if (shouldFail)
                {
                    result.Ciphertext = _rand.GetDifferentBitStringOfSameSize(result.Ciphertext);
                    result.TestPassed = false;
                }
            }

            // Notify observers of result
            await Notify(result);
        }
    }
}