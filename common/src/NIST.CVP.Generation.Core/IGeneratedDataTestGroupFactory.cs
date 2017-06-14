using System;
using System.Collections.Generic;

namespace NIST.CVP.Generation.Core
{
    [Obsolete("To be replaced by more generic ITestGroupGenerator")]
    public interface IGeneratedDataTestGroupFactory<in TParameters, out TTestGroup>
        where TParameters : IParameters
        where TTestGroup : ITestGroup
    {
        IEnumerable<TTestGroup> BuildGDTTestGroups(TParameters parameters);
    }
}
