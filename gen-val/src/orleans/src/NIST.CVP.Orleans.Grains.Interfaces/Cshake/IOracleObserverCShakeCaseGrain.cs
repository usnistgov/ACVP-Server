﻿using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Cshake
{
    public interface IOracleObserverCShakeCaseGrain : IGrainWithGuidKey, IGrainObservable<CShakeResult>
    {
        Task<bool> BeginWorkAsync(CShakeParameters param);
    }
}