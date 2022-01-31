using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KMAC.v1_0
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
                foreach (var xof in parameters.XOF)
                {
                    foreach (var hexCustomization in hexCustomizations)
                    {
                        var testGroup = new TestGroup
                        {
                            DigestSize = digestSize,
                            TestType = TEST_TYPE,
                            MsgLengths = parameters.MsgLen.GetDeepCopy(),
                            KeyLengths = parameters.KeyLen.GetDeepCopy(),
                            MacLengths = parameters.MacLen.GetDeepCopy(),
                            HexCustomization = hexCustomization,
                            XOF = xof
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }

            return Task.FromResult(testGroups);
        }
    }
}
