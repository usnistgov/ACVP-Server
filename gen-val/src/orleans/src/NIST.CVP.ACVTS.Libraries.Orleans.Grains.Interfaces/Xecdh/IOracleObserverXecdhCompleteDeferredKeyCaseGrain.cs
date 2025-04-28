﻿using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Xecdh
{
    public interface IOracleObserverXecdhCompleteDeferredKeyCaseGrain : IGrainWithGuidKey, IGrainObservable<XecdhKeyResult>
    {
        Task<bool> BeginWorkAsync(XecdhKeyParameters param, XecdhKeyResult fullParam);
    }
}
