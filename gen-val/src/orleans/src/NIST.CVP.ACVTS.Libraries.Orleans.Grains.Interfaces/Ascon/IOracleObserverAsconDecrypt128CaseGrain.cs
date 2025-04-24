using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Ascon;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Ascon;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Ascon;

public interface IOracleObserverAsconDecrypt128CaseGrain : IGrainWithGuidKey, IGrainObservable<AsconAead128Result>
{
    Task<bool> BeginWorkAsync(AsconAEAD128Parameters parameters);
}
