using System.Collections.Generic;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeTestGroupGenerator : ITestGroupGenerator<IParameters>
    {
        public IEnumerable<ITestGroup> BuildTestGroups(IParameters parameters)
        {
            return null;
        }
    }
}
