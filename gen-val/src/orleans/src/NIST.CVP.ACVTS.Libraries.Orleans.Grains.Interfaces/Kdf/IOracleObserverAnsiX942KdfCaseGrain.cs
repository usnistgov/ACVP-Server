using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kdf
{
    public interface IOracleObserverAnsiX942KdfCaseGrain : IGrainWithGuidKey, IGrainObservable<AnsiX942KdfResult>
    {
        Task<bool> BeginWorkAsync(AnsiX942Parameters param);
    }
}
