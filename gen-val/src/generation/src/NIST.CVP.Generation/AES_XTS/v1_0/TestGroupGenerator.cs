using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.AES_XTS.v1_0
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE_LABEL = "AFT";

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            parameters.PayloadLen.SetRangeOptions(RangeDomainSegmentOptions.Random);
            var minMaxPtLen = parameters.PayloadLen.GetDomainMinMax();

            foreach (var function in parameters.Direction)
            {
                foreach (var keyLen in parameters.KeyLen)
                {
                    foreach (var tweakMode in parameters.TweakModes)
                    {
                        var testPtLens = new List<int>();

                        testPtLens.AddRange(parameters.PayloadLen.GetValues(w => w % 128 == 0 && w != minMaxPtLen.Maximum, 2, true));
                        testPtLens.AddRange(parameters.PayloadLen.GetValues(w => w % 128 != 0 && w != minMaxPtLen.Maximum, 2, true));
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

            return Task.FromResult(testGroups.AsEnumerable());
        }
    }
}
