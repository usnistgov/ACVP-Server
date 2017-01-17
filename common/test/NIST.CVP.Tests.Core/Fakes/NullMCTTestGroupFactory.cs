using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Tests.Core.Fakes
{
    public class NullMCTTestGroupFactory : IMCTTestGroupFactory<IParameters, IEnumerable<ITestGroup>>
    {
        public IEnumerable<ITestGroup> BuildMCTTestGroups(IParameters parameters)
        {
            return null;
        }
    }
}
