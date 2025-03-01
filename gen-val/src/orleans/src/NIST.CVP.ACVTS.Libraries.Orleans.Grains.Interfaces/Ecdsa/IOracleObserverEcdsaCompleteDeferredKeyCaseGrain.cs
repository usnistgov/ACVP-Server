﻿using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Ecdsa
{
    public interface IOracleObserverEcdsaCompleteDeferredKeyCaseGrain : IGrainWithGuidKey, IGrainObservable<EcdsaKeyResult>
    {
        Task<bool> BeginWorkAsync(EcdsaKeyParameters param, EcdsaKeyResult fullParam);
    }
}
