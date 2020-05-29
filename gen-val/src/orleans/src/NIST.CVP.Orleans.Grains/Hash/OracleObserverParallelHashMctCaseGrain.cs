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
    public class OracleObserverParallelHashMctCaseGrain : ObservableOracleGrainBase<MctResult<ParallelHashResult>>, 
        IOracleObserverParallelHashMctCaseGrain
    {
        private readonly IParallelHash_MCT _hash;
        private readonly IRandom800_90 _rand;

        private ParallelHashParameters _param;

        public OracleObserverParallelHashMctCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IParallelHash_MCT hash,
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
            var result = _hash.MCTHash(_param.HashFunction, message, _param.OutLens, _param.HexCustomization, _param.IsSample);

            if (!result.Success)
            {
                throw new Exception();
            }

            await Notify(new MctResult<ParallelHashResult>
            {
                Seed = new ParallelHashResult
                {
                    Message = message,
                    Customization = "",    // Statically set in the Crypto MCT for ParallelHash
                    BlockSize = 8,         // Statically set in the Crypto MCT for ParallelHash
                    FunctionName = _param.FunctionName
                },
                Results = result.Response.ConvertAll(element =>
                    new ParallelHashResult
                    {
                        Message = element.Message, 
                        Digest = element.Digest, 
                        Customization = element.Customization
                    })
            });
        }
    }
}
