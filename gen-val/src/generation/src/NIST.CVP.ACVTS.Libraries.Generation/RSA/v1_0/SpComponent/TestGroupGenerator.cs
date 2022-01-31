using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SpComponent
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            return Task.FromResult(new List<TestGroup>
            {
                new TestGroup
                {
                    KeyFormat = parameters.KeyFormat,
                    PublicExponentMode = parameters.PublicExponentMode,
                    PublicExponent = parameters.PublicExponent
                }
            });
        }
    }
}
