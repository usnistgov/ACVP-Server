﻿using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Aead;

namespace NIST.CVP.Orleans.Grains.Aead
{
    public class OracleObserverAesDeferredGcmCaseGrain : ObservableOracleGrainBase<AeadResult>, 
        IOracleObserverAesDeferredGcmCaseGrain
    {
        private readonly IRandom800_90 _rand;
        
        private AeadParameters _param;

        public OracleObserverAesDeferredGcmCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IRandom800_90 rand
        ) : base (nonOrleansScheduler)
        {
            _rand = rand;
        }
        
        public async Task<bool> BeginWorkAsync(AeadParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            // Notify observers of result
            await Notify(new AeadResult
            {
                Aad = _rand.GetRandomBitString(_param.AadLength),
                PlainText = _rand.GetRandomBitString(_param.PayloadLength),
                Key = _rand.GetRandomBitString(_param.KeyLength)
            });
        }
    }
}