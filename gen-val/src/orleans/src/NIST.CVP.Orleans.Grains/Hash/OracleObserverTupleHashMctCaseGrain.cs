using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Hash;

namespace NIST.CVP.Orleans.Grains.Hash
{
    public class OracleObserverTupleHashMctCaseGrain : ObservableOracleGrainBase<MctResult<TupleHashResult>>, 
        IOracleObserverTupleHashMctCaseGrain
    {
        private readonly ITupleHash_MCT _hash;
        private readonly IRandom800_90 _rand;

        private TupleHashParameters _param;

        public OracleObserverTupleHashMctCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ITupleHash_MCT hash,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _hash = hash;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(TupleHashParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var initialLength = _param.InputLengths.GetValues(0, 65536, 3).ToList().Shuffle().First();
            var tuple = new List<BitString>() { _rand.GetRandomBitString(initialLength) };

            var result = _hash.MCTHash(_param.HashFunction, tuple, _param.OutputLengths, _param.HexCustomization, _param.IsSample);

            if (!result.Success)
            {
                throw new Exception();
            }

            await Notify(new MctResult<TupleHashResult>
            {
                Seed = new TupleHashResult 
                {
                    Tuple = tuple, 
                    Customization = "", 
                    FunctionName = _param.FunctionName
                },
                Results = result.Response.ConvertAll(element =>
                    new TupleHashResult { Tuple = element.Tuple, Digest = element.Digest, Customization = element.Customization })
            });
        }
    }
}
