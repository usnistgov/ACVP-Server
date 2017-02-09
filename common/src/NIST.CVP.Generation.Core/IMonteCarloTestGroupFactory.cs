using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.Core
{
    public interface IMonteCarloTestGroupFactory<in TParameters, out TTestGroup>
        where TParameters : IParameters
        where TTestGroup : ITestGroup
    {
        IEnumerable<TTestGroup> BuildMCTTestGroups(TParameters parameters);
    }
}
