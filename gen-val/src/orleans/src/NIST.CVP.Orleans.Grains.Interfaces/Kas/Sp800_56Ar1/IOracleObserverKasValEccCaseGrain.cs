﻿using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Ar1
{
    public interface IOracleObserverKasValEccCaseGrain : IGrainWithGuidKey, IGrainObservable<KasValResultEcc>
    {
        Task<bool> BeginWorkAsync(KasValParametersEcc param);
    }
}