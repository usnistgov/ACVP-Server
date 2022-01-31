using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CCM.v1_0
{
    public class TestGroupGeneratorEcma : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const int NONCE_LEN = 13 * 8;
        private const int TAG_LEN = 8 * 8;
        private const int PT_LEN = 32 * 8;
        private const int KEY_LEN = 128;
        private const int AAD_MIN_LEN = 14 * 8;
        private const int AAD_MAX_LEN = 32 * 8;
        private readonly string[] DIRECTIONS = { "encrypt", "decrypt" };

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            // AAD is required (by ParameterValidator) to be on the byte boundaries so there are only 19 possible values here
            var aadLens = parameters.AadLen.GetDeepCopy().GetValues(aad => aad >= AAD_MIN_LEN && aad <= AAD_MAX_LEN && aad % 8 == 0, 20, false).ToArray();

            if (aadLens.Length == 0)
            {
                throw new Exception("Empty group in ECMA");
            }

            foreach (var direction in DIRECTIONS)
            {
                var testGroupAft = new TestGroup
                {
                    Function = direction,
                    KeyLength = KEY_LEN,
                    PayloadLength = PT_LEN,
                    TagLength = TAG_LEN,
                    InternalTestType = "ECMA-AFT",
                    AADLength = AAD_MAX_LEN
                };

                testGroups.Add(testGroupAft);

                var testGroupVadt = new TestGroup
                {
                    Function = direction,
                    KeyLength = KEY_LEN,
                    PayloadLength = PT_LEN,
                    TagLength = TAG_LEN,
                    InternalTestType = "ECMA-VADT",
                    AADLengths = aadLens
                };

                testGroups.Add(testGroupVadt);
            }

            return Task.FromResult(testGroups);
        }
    }
}
