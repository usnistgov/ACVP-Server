﻿using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Aes
{
    public interface IOracleObserverAesXtsCaseGrain : IGrainWithGuidKey, IGrainObservable<AesXtsResult>
    {
        Task<bool> BeginWorkAsync(AesXtsParameters param);
    }
}
