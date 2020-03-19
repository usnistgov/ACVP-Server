using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.AES_XPN.v1_0
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            parameters.PayloadLen.SetRangeOptions(RangeDomainSegmentOptions.Random);
            parameters.AadLen.SetRangeOptions(RangeDomainSegmentOptions.Random);
            
            var ptLengths = new List<int>();
            ptLengths.AddRangeIfNotNullOrEmpty(parameters.PayloadLen.GetDomainMinMaxAsEnumerable());
            // Get block length values
            ptLengths.AddRangeIfNotNullOrEmpty(
                parameters.PayloadLen
                    .GetValues(g => g % 128 == 0 && !ptLengths.Contains(g), 2, true));
            // get non block length values
            ptLengths.AddRangeIfNotNullOrEmpty(
                parameters.PayloadLen
                    .GetValues(g => g % 8 == 0 && g % 128 != 0 && !ptLengths.Contains(g), 2, true));

            var aadLengths = GetTestableValuesFromCapability(parameters.AadLen);

            var tagLengths = new List<int>();
            foreach (var validTagLength in ParameterValidator.VALID_TAG_LENGTHS)
            {
                if (parameters.TagLen.IsWithinDomain(validTagLength))
                {
                    tagLengths.Add(validTagLength);
                }
            }

            // sanity check, should be caught by parameter validator
            if (tagLengths.Count == 0)
            {
                throw new ArgumentException("No valid tag lengths found within parameters");
            }

            foreach (var function in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                        foreach (var ptLength in ptLengths)
                        {
                            foreach (var aadLength in aadLengths)
                            {
                                foreach (var tagLength in tagLengths)
                                {
                                var testGroup = new TestGroup
                                {
                                    Function = function,
                                    PayloadLength = ptLength,
                                    KeyLength = keyLength,
                                    AadLength = aadLength,
                                    TagLength = tagLength,
                                    IvGeneration = parameters.IvGen,
                                    IvGenerationMode = parameters.IvGenMode,
                                    SaltGen = parameters.SaltGen
                                };
                                testGroups.Add(testGroup);
                            }
                        }
                    }
                }
            }
            return testGroups;
        }

        private List<int> GetTestableValuesFromCapability(MathDomain capability)
        {
            var minMaxDomain = capability.GetDomainMinMaxAsEnumerable();
            var randomCandidates = capability.GetValues(10).ToList();
            var testValues = new List<int>();
            testValues.AddRangeIfNotNullOrEmpty(minMaxDomain.Distinct());
            testValues
                .AddRangeIfNotNullOrEmpty(
                    randomCandidates
                        .Except(testValues)
                        .OrderBy(ob => Guid.NewGuid())
                        .Take(2)
                );

            return testValues;
        }
    }
}