using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v2_0
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE_LABEL = "AFT";
        private const string SINGLE_DATA_UNIT = "SingleDataUnit";
        private const string MULTIPLE_DATA_UNIT = "MultipleDataUnit";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var function in parameters.Direction)
            {
                foreach (var keyLen in parameters.KeyLen)
                {
                    foreach (var tweakMode in parameters.TweakMode)
                    {
                        var singleTestGroup = new TestGroup
                        {
                            Direction = function,
                            KeyLen = keyLen,
                            TweakMode = tweakMode,
                            PayloadLen = parameters.PayloadLen.GetDeepCopy(),
                            TestType = TEST_TYPE_LABEL,
                            InternalTestType = SINGLE_DATA_UNIT,
                        };

                        testGroups.Add(singleTestGroup);

                        if (!parameters.DataUnitLenMatchesPayload)
                        {
                            var multiTestGroup = new TestGroup
                            {
                                Direction = function,
                                KeyLen = keyLen,
                                TweakMode = tweakMode,
                                PayloadLen = parameters.PayloadLen.GetDeepCopy(),
                                DataUnitLen = parameters.DataUnitLen.GetDeepCopy(),
                                TestType = TEST_TYPE_LABEL,
                                InternalTestType = MULTIPLE_DATA_UNIT
                            };

                            testGroups.Add(multiTestGroup);
                        }
                    }
                }
            }

            return Task.FromResult(testGroups);
        }
    }
}
