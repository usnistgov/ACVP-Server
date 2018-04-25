using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            parameters.ivLen.SetRangeOptions(RangeDomainSegmentOptions.Random);
            parameters.PtLen.SetRangeOptions(RangeDomainSegmentOptions.Random);
            parameters.aadLen.SetRangeOptions(RangeDomainSegmentOptions.Random);

            // iv length of 96 is a special case, if it's in the domain, include it.
            var ivLengths = new List<int>();
            var ivLengthMinMax = parameters.ivLen.GetDomainMinMax();
            const int ivLenSpecialCase = 96;
            if (ivLengthMinMax.Minimum < ivLenSpecialCase)
            {
                ivLengths.AddRangeIfNotNullOrEmpty(parameters.ivLen.GetValues(ivLengthMinMax.Minimum,
                    ivLenSpecialCase - 1, 2));
            }
            if (ivLengthMinMax.Maximum >= ivLenSpecialCase)
            {
                ivLengths.AddRangeIfNotNullOrEmpty(parameters.ivLen.GetValues(ivLenSpecialCase, ivLenSpecialCase, 1));
            }
            if (ivLengthMinMax.Maximum > ivLenSpecialCase)
            {
                ivLengths.AddRangeIfNotNullOrEmpty(parameters.ivLen.GetValues(ivLenSpecialCase + 1,
                    ivLengthMinMax.Maximum, 2));
            }

            var ptLengths = new List<int>();
            ptLengths.AddRangeIfNotNullOrEmpty(parameters.PtLen.GetDomainMinMaxAsEnumerable());
            // Get block length values
            ptLengths.AddRangeIfNotNullOrEmpty(
                parameters.PtLen
                    .GetValues(g => g % 128 == 0 && !ptLengths.Contains(g), 2, true));
            // get non block length values
            ptLengths.AddRangeIfNotNullOrEmpty(
                parameters.PtLen
                    .GetValues(g => g % 8 == 0 && g % 128 != 0 && !ptLengths.Contains(g), 2, true));

            var aadLengths = GetTestableValuesFromCapability(parameters.aadLen);

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
                    foreach (var ivLength in ivLengths)
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
                                        IVLength = ivLength,
                                        PTLength = ptLength,
                                        KeyLength = keyLength,
                                        AADLength = aadLength,
                                        TagLength = tagLength,
                                        IVGeneration = parameters.ivGen,
                                        IVGenerationMode = parameters.ivGenMode

                                    };
                                    testGroups.Add(testGroup);
                                }
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