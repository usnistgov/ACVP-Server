﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_OFB.v1_0
{
    public class TestGroupGeneratorMonteCarlo : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string _MCT_TEST_TYPE_LABEL = "MCT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var direction in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    var testGroup = new TestGroup()
                    {
                        Function = direction,
                        KeyLength = keyLength,
                        InternalTestType = _MCT_TEST_TYPE_LABEL,
                        TestType = _MCT_TEST_TYPE_LABEL
                    };
                    testGroups.Add(testGroup);
                }
            }

            return Task.FromResult(testGroups);
        }
    }
}
