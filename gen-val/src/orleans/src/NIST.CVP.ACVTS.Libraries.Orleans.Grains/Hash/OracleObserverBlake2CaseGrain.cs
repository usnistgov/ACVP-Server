using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.Blake2;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Hash;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Hash
{
    public class OracleObserverBlake2CaseGrain : ObservableOracleGrainBase<Blake2Result>,
        IOracleObserverBlake2CaseGrain
    {
        private readonly IBlake2Factory _blake2Factory;
        private readonly IRandom800_90 _rand;

        private Blake2Parameters _param;

        public OracleObserverBlake2CaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IBlake2Factory blake2Factory,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _blake2Factory = blake2Factory;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(Blake2Parameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var message = _rand.GetRandomBitString(_param.MessageLength);
            var key = _param.KeyLength == 0 ? null : _rand.GetRandomBitString(_param.KeyLength);

            var result = _blake2Factory.GetBlake2Instance(_param.HashFunction)
                .HashMessage(message, key);
            if (!result.Success)
            {
                throw new Exception(result.ErrorMessage);
            }

            await Notify(new Blake2Result
            {
                Message = message,
                Key = key,
                Digest = result.Digest
            });
        }
    }
}
