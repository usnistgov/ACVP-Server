using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.EccComponent
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string testType = "AFT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            List<TestGroup> groups = new List<TestGroup>();
            foreach (var curveString in parameters.Curves)
            {
                var curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveString);

                var group = new TestGroup()
                {
                    CurveName = curve,
                    TestType = testType
                };
                groups.Add(group);
            }

            return groups;
        }
    }
}