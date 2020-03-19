using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CCM.v1_0
{
    public class TestGroupGenerator80211 : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
