using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Tests.Core.Fakes
{
    public class NullMCTTestGroupFactory : IMonteCarloTestGroupFactory<IParameters, ITestGroup>
    {
        public IEnumerable<ITestGroup> BuildMCTTestGroups(IParameters parameters)
        {
            return null;
        }
    }
}
