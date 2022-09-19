using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.cSHAKE.v1_0
{
    public class TestGroupGeneratorAlgorithmFunctional : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            // false means test with ascii customization, true means test with hex customization
            var hexCustomizations = new List<bool>() { false };
            if (parameters.HexCustomization)
            {
                hexCustomizations.Add(true);
            }

            foreach (var digestSize in parameters.DigestSizes)
            {
                foreach (var hexCustomization in hexCustomizations)
                {
                    var testGroup = new TestGroup
                    {
                        Function = "cSHAKE",
                        DigestSize = digestSize,
                        MessageLength = parameters.MessageLength.GetDeepCopy(),
                        HexCustomization = hexCustomization,
                        OutputLength = parameters.OutputLength.GetDeepCopy(),
                        TestType = TEST_TYPE
                    };

                    testGroups.Add(testGroup);
                }
            }

            return Task.FromResult(testGroups);
        }
    }
}
