using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kdf
{
    public interface IOracleObserverAnsiX942KdfCaseGrain : IGrainWithGuidKey, IGrainObservable<AnsiX942KdfResult>
    {
        Task<bool> BeginWorkAsync(AnsiX942Parameters param);
    }
}
