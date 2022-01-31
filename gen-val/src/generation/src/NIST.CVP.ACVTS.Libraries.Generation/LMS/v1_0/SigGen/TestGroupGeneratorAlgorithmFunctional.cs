using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigGen
{
    public class TestGroupGeneratorAlgorithmFunctional : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";

        private readonly Random _rand;

        public TestGroupGeneratorAlgorithmFunctional()
        {
            _rand = new Random();
        }

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new HashSet<TestGroup>();

            if (parameters.Specific)
            {
                for (int i = 0; i < parameters.SpecificCapabilities.Length; i++)
                {
                    var lmsTypes = new List<LmsType>();
                    var lmotsTypes = new List<LmotsType>();
                    foreach (var level in parameters.SpecificCapabilities[i].Levels)
                    {
                        lmsTypes.Add(EnumHelpers.GetEnumFromEnumDescription<LmsType>(level.LmsType));
                        lmotsTypes.Add(EnumHelpers.GetEnumFromEnumDescription<LmotsType>(level.LmotsType));
                    }
                    var testGroup = new TestGroup
                    {
                        TestType = TEST_TYPE,
                        LmsTypes = lmsTypes,
                        LmotsTypes = lmotsTypes
                    };

                    testGroups.Add(testGroup);
                }
            }
            else
            {
                for (int height = 1; height <= 3; height++)        // Max Layers is 3 
                {
                    foreach (var lmsType in parameters.Capabilities.LmsTypes)
                    {
                        foreach (var lmotsType in parameters.Capabilities.LmotsTypes)
                        {
                            var lmsTypes = new LmsType[height];
                            var lmotsTypes = new LmotsType[height];
                            for (int j = 0; j < height; j++)
                            {
                                lmsTypes[j] = EnumHelpers.GetEnumFromEnumDescription<LmsType>(lmsType);
                                lmotsTypes[j] = EnumHelpers.GetEnumFromEnumDescription<LmotsType>(lmotsType);
                            }

                            var testGroup = new TestGroup
                            {
                                TestType = TEST_TYPE,
                                LmsTypes = new List<LmsType>(lmsTypes),
                                LmotsTypes = new List<LmotsType>(lmotsTypes)
                            };

                            testGroups.Add(testGroup);
                        }
                    }

                    var currentCount = testGroups.Count;

                    // There are ((lmsTypes.Length ^ height) - lmsTypes.Length) * ((lmotsTypes.Length ^ height) - lmotsTypes.Length)
                    // possible additional unique test groups
                    var lmsNumPow = 1;
                    var lmsNum = parameters.Capabilities.LmsTypes.Length;
                    for (int i = 1; i <= height; i++)
                    {
                        lmsNumPow *= lmsNum;
                    }
                    var lmotsNumPow = 1;
                    var lmotsNum = parameters.Capabilities.LmotsTypes.Length;
                    for (int i = 1; i <= height; i++)
                    {
                        lmotsNumPow *= lmotsNum;
                    }
                    var remainingUniqueGroups = (lmsNumPow - lmsNum) * (lmotsNumPow - lmotsNum);

                    // while total test groups generated is less than 7 and it is impossible to make a unique test group
                    while (testGroups.Count - currentCount < 7 && testGroups.Count - currentCount < remainingUniqueGroups)
                    {
                        var lmsTypes = new LmsType[height];
                        var lmotsTypes = new LmotsType[height];
                        for (int i = 0; i < height; i++)
                        {
                            lmsTypes[i] = EnumHelpers.GetEnumFromEnumDescription<LmsType>(
                                parameters.Capabilities.LmsTypes[_rand.Next(parameters.Capabilities.LmsTypes.Length)]);
                            lmotsTypes[i] = EnumHelpers.GetEnumFromEnumDescription<LmotsType>(
                                parameters.Capabilities.LmotsTypes[_rand.Next(parameters.Capabilities.LmotsTypes.Length)]);
                        }

                        var testGroup = new TestGroup
                        {
                            TestType = TEST_TYPE,
                            LmsTypes = new List<LmsType>(lmsTypes),
                            LmotsTypes = new List<LmotsType>(lmotsTypes)
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }

            return Task.FromResult(testGroups.ToList());
        }
    }
}
