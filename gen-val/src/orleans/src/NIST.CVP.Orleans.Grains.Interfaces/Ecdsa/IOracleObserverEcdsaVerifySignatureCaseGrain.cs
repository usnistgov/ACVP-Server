﻿using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Ecdsa
{
    public interface IOracleObserverEcdsaVerifySignatureCaseGrain : IGrainWithGuidKey, IGrainObservable<VerifyResult<EcdsaSignatureResult>>
    {
        Task<bool> BeginWorkAsync(EcdsaSignatureParameters param, EcdsaKeyResult key, EcdsaKeyResult badKey);
    }
}