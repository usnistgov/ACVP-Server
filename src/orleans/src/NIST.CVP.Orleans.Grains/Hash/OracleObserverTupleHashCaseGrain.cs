﻿using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var tuple = _param.MessageLength.Select(msgLen => _rand.GetRandomBitString(msgLen)).ToList();

            Crypto.Common.Hash.HashResult result;
            BitString customizationHex = null;
            var customization = "";
            
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
                throw new Exception(result.ErrorMessage);
            }
            
            await Notify(new TupleHashResult
            {
                Tuple = tuple,
                Digest = result.Digest,
                Customization = customization,
                CustomizationHex = customizationHex
            });
        }
    }
}
