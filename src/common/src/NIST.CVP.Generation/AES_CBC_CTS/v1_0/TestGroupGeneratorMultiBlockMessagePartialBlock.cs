using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CBC_CTS.v1_0
{
    public class TestGroupGeneratorMultiBlockMessagePartialBlock : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const string MMT_TYPE_LABEL = "AFT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var function in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    var testGroup = new TestGroup
                    {
                        Function = function,
                        KeyLength = keyLength,
                        TestType = MMT_TYPE_LABEL,
                        InternalTestType = "MMT",
                        IsPartialBlockGroup = true,
                        PayloadLen = parameters.PayloadLen.GetDeepCopy()
                    };
                    testGroups.Add(testGroup);
                }
            }
            return testGroups;
        }
    }
}
