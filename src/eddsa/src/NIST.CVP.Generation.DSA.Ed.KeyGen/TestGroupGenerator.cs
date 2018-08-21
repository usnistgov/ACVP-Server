using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.Ed.KeyGen
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var curveParam in parameters.Curve)
            {
                foreach (var secretModeParam in parameters.SecretGenerationMode)
                {
                    var testGroup = new TestGroup
                    {
                        Curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveParam),
                        SecretGenerationMode = EnumHelpers.GetEnumFromEnumDescription<SecretGenerationMode>(secretModeParam)
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}
