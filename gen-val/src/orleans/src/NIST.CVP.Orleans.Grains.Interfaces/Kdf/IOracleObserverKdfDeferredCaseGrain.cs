﻿using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kdf
{
    public interface IOracleObserverKdfDeferredCaseGrain : IGrainWithGuidKey, IGrainObservable<KdfResult>
    {
        Task<bool> BeginWorkAsync(KdfParameters param);
    }
}