﻿using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Eddsa
{
    public interface IOracleObserverEddsaCompleteDeferredKeyCaseGrain : IGrainWithGuidKey, IGrainObservable<EddsaKeyResult>
    {
        Task<bool> BeginWorkAsync(EddsaKeyParameters param, EddsaKeyResult fullParam);
    }
}