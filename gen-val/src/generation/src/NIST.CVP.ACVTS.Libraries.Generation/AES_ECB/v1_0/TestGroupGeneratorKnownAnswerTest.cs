﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_ECB.v1_0
{
    public class TestGroupGeneratorKnownAnswerTests : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private readonly string[] _katTests = new string[]
        {
            "GFSBox",
            "KeySBox",
            "VarTxt",
            "VarKey"
        };

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var direction in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    foreach (var katTest in _katTests)
                    {
                        var testGroup = new TestGroup()
                        {
                            Function = direction,
                            KeyLength = keyLength,
                            InternalTestType = katTest
                        };
                        testGroups.Add(testGroup);
                    }
                }
            }

            return Task.FromResult(testGroups);
        }
    }
}
