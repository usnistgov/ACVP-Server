using System.Collections.Generic;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeTestGroupGenerator : ITestGroupGeneratorAsync<FakeParameters, FakeTestGroup, FakeTestCase>
    {
        public Task<IEnumerable<FakeTestGroup>> BuildTestGroupsAsync(FakeParameters parameters)
        {
            return null;
        }
    }
}
