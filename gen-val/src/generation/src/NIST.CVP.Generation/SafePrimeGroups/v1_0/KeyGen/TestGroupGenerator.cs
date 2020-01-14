using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SafePrimeGroups.v1_0.KeyGen
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var safePrimeGroup in parameters.SafePrimeGroups)
            {
                var testGroup = new TestGroup
                {
                    SafePrimeGroup = safePrimeGroup
                };

                testGroups.Add(testGroup);
            }

            return testGroups;
        }
    }
}