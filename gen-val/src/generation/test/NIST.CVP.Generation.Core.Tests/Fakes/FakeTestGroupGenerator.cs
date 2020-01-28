using System.Collections.Generic;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeTestGroupGenerator : ITestGroupGenerator<FakeParameters, FakeTestGroup, FakeTestCase>
    {
        public IEnumerable<FakeTestGroup> BuildTestGroups(FakeParameters parameters)
        {
            return null;
        }
    }
}
