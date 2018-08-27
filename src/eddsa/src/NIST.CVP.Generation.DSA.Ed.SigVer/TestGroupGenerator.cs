using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.Ed.SigVer.TestCaseExpectations;
using System.Collections.Generic;

namespace NIST.CVP.Generation.DSA.Ed.SigVer
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            // Use a hash set because the registration allows for duplicate pairings to occur
            // Equality of groups is done via name of the curve and name of the hash function.
            // HashSet eliminates any duplicates that may be registered
            var testGroups = new HashSet<TestGroup>();

            foreach (var curveName in parameters.Curve)
            {
                var curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveName);

                if (parameters.Pure)
                {
                    var testGroup = new TestGroup
                    {
                        TestCaseExpectationProvider = new TestCaseExpectationProvider(parameters.IsSample),
                        Curve = curve,
                        PreHash = false
                    };

                    testGroups.Add(testGroup);
                }

                if (parameters.PreHash)
                {
                    var testGroup = new TestGroup
                    {
                        TestCaseExpectationProvider = new TestCaseExpectationProvider(parameters.IsSample),
                        Curve = curve,
                        PreHash = true
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}
