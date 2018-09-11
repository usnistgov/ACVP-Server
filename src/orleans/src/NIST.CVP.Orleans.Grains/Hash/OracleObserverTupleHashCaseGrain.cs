using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Math.Entropy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Hash;

namespace NIST.CVP.Orleans.Grains.Hash
{
    public class OracleObserverTupleHashCaseGrain : ObservableOracleGrainBase<TupleHashResult>, 
        IOracleObserverTupleHashCaseGrain
    {
        private readonly ITupleHash _hash;
        private readonly IRandom800_90 _rand;

        private TupleHashParameters _param;

        public OracleObserverTupleHashCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            ITupleHash hash,
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
            var tuple = new List<BitString>();

            if (_param.SemiEmptyCase)
            {
                for (int i = 0; i < _param.TupleSize; i++)
                {
                    if (_rand.GetRandomInt(0, 2) == 1)  // either 1 or 0
                    {
                        tuple.Add(_rand.GetRandomBitString(GetRandomValidLength(_param.BitOrientedInput)));
                    }
                    else
                    {
                        tuple.Add(new BitString(""));
                    }
                }
            }
            else if (_param.LongRandomCase)
            {
                for (int i = 0; i < _param.TupleSize; i++)
                {
                    tuple.Add(_rand.GetRandomBitString(GetRandomValidLength(_param.BitOrientedInput)));
                }
            }
            else
            {
                for (int i = 0; i < _param.TupleSize; i++)
                {
                    tuple.Add(_rand.GetRandomBitString(_param.MessageLength));
                }
            }
            
            Crypto.Common.Hash.HashResult result;
            BitString customizationHex = null;
            string customization = "";
            if (_param.HexCustomization)
            {
                customizationHex = _rand.GetRandomBitString(_param.CustomizationLength);
                result = _hash.HashMessage(_param.HashFunction, tuple, customizationHex);
            }
            else
            {
                customization = _rand.GetRandomString(_param.CustomizationLength);
                result = _hash.HashMessage(_param.HashFunction, tuple, customization);
            }

            if (!result.Success)
            {
                throw new Exception();
            }
            
            await Notify(new TupleHashResult
            {
                Tuple = tuple,
                Digest = result.Digest,
                Customization = customization,
                CustomizationHex = customizationHex
            });
        }

        private int GetRandomValidLength(bool bitOriented)
        {
            var length = _rand.GetRandomInt(1, 513);
            if (!bitOriented)
            {
                while (length % 8 != 0)
                {
                    length++;
                }
            }
            return length;
        }
    }
}
