﻿using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Rsa
{
    public interface IOracleObserverRsaVerifyKeyCaseGrain : IGrainWithGuidKey, IGrainObservable<VerifyResult<RsaKeyResult>>
    {
        Task<bool> BeginWorkAsync(RsaKeyResult param);
    }
}