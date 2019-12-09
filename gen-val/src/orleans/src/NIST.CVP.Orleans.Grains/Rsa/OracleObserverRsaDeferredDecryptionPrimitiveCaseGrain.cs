﻿using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.Orleans.Grains.Rsa
{
    public class OracleObserverRsaDeferredDecryptionPrimitiveCaseGrain : ObservableOracleGrainBase<RsaDecryptionPrimitiveResult>, 
        IOracleObserverRsaDeferredDecryptionPrimitiveCaseGrain
    {
        private readonly IRandom800_90 _rand;

        private RsaDecryptionPrimitiveParameters _param;

        public OracleObserverRsaDeferredDecryptionPrimitiveCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _rand = rand;
        }
        
        public async Task<bool> BeginWorkAsync(RsaDecryptionPrimitiveParameters param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var result =  new RsaDecryptionPrimitiveResult
            {
                CipherText = _param.TestPassed
                    ? _rand.GetRandomBitString(_param.Modulo)
                    : BitString.Ones(2).ConcatenateBits(_rand.GetRandomBitString(_param.Modulo - 2)) // Try to force the failing case high
            };

            // Notify observers of result
            await Notify(result);
        }
    }
}