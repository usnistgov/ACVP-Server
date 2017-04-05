using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Tests.Core.Fakes
{
    public class NullMCTTestGroupFactory<TParameters, TTestGroup> : IMonteCarloTestGroupFactory<TParameters, TTestGroup>
        where TParameters : IParameters
        where TTestGroup : ITestGroup
    {
        public IEnumerable<TTestGroup> BuildMCTTestGroups(TParameters parameters)
        {
            return null;
        }
    }
}
