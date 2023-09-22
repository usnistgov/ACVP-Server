using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v1_0
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE_LABEL = "AFT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            var minMaxPtLen = parameters.PayloadLen.GetDomainMinMax();

            foreach (var function in parameters.Direction)
            {
                foreach (var keyLen in parameters.KeyLen)
                {
                    foreach (var tweakMode in parameters.TweakMode)
                    {
                        var testPtLens = new List<int>();

                        testPtLens.AddRange(parameters.PayloadLen.GetRandomValues(w => w % 128 == 0 && w != minMaxPtLen.Maximum, 2));
                        testPtLens.AddRange(parameters.PayloadLen.GetRandomValues(w => w % 128 != 0 && w != minMaxPtLen.Maximum, 2));
                        testPtLens.Add(minMaxPtLen.Maximum);

                        foreach (var ptLen in testPtLens)
                        {
                            var testGroup = new TestGroup
                            {
                                Direction = function,
                                KeyLen = keyLen,
                                TweakMode = tweakMode,
                                PayloadLen = ptLen,
                                TestType = TEST_TYPE_LABEL
                            };

                            testGroups.Add(testGroup);
                        }
                    }
                }
            }

            return Task.FromResult(testGroups);
        }
    }
}
