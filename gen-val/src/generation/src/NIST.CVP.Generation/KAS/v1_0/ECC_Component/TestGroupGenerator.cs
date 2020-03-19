using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.v1_0.ECC_Component
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string testType = "AFT";

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            List<TestGroup> groups = new List<TestGroup>();
            foreach (var curveString in parameters.Curve)
            {
                var curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveString);

                var group = new TestGroup()
                {
                    Curve = curve,
                    TestType = testType
                };
                groups.Add(group);
            }

            return Task.FromResult(groups.AsEnumerable());
        }
    }
}