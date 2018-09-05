using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.Orleans.Grains.Interfaces.Hash;
using NIST.CVP.Math;
using HashResult = NIST.CVP.Crypto.Common.Hash.HashResult;

namespace NIST.CVP.Orleans.Grains.Hash
{
    public class OracleObserverParallelHashCaseGrain : ObservableOracleGrainBase<ParallelHashResult>, 
        IOracleObserverParallelHashCaseGrain
    {
        private readonly IParallelHash _hash;
        private readonly IRandom800_90 _rand;

        private ParallelHashParameters _param;

        public OracleObserverParallelHashCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IParallelHash hash,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _hash = hash;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(ParallelHashParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var message = _rand.GetRandomBitString(_param.MessageLength);

            HashResult result;
            BitString customizationHex = null;
            string customization = "";
            if (_param.HexCustomization)
            {
                customizationHex = _rand.GetRandomBitString(_param.CustomizationLength);
                result = _hash.HashMessage(_param.HashFunction, message, _param.BlockSize, customizationHex);
            }
            else
            {
                customization = _rand.GetRandomString(_param.CustomizationLength);
                result = _hash.HashMessage(_param.HashFunction, message, _param.BlockSize, customization);
            }

            if (!result.Success)
            {
                throw new Exception();
            }

            await Notify(new ParallelHashResult
            {
                Message = message,
                Digest = result.Digest,
                Customization = customization,
                CustomizationHex = customizationHex,
                BlockSize = _param.BlockSize
            });
        }
    }
}
