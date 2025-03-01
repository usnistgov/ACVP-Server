﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CBC_CTS.v1_0
{
    public class TestGroupGeneratorMultiBlockMessagePartialBlock : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string MMT_TYPE_LABEL = "AFT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
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
                        PayloadLen = parameters.PayloadLen.GetDeepCopy(),
                        AlgoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision)
                    };
                    testGroups.Add(testGroup);
                }
            }
            return Task.FromResult(testGroups);
        }
    }
}
