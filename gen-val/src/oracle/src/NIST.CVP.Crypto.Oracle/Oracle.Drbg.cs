﻿using NIST.CVP.Crypto.Common.DRBG;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Drbg;
using DrbgResult = NIST.CVP.Common.Oracle.ResultTypes.DrbgResult;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<DrbgResult> GetDrbgCaseAsync(DrbgParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverDrbgCaseGrain, DrbgResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }
    }
}