﻿using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Ecdsa
{
    public interface IOracleObserverEcdsaCompleteDeferredKeyCaseGrain : IGrainWithGuidKey, IGrainObservable<EcdsaKeyResult>
    {
        Task<bool> BeginWorkAsync(EcdsaKeyParameters param, EcdsaKeyResult fullParam);
    }
}