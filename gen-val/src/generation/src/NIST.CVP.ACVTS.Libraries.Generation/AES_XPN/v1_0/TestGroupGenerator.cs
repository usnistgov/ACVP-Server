using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_XPN.v1_0
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            var ptLengths = new List<int>();
            ptLengths.AddRangeIfNotNullOrEmpty(parameters.PayloadLen.GetDomainMinMaxAsEnumerable());

            // Get block length values
            ptLengths.AddRangeIfNotNullOrEmpty(parameters.PayloadLen.GetRandomValues(g => g % 128 == 0 && !ptLengths.Contains(g), 2));

            // Get non block length values
            ptLengths.AddRangeIfNotNullOrEmpty(parameters.PayloadLen.GetRandomValues(g => g % 8 == 0 && g % 128 != 0 && !ptLengths.Contains(g), 2));

            var aadLengths = GetTestableValuesFromCapability(parameters.AadLen);
            var tagLengths = parameters.TagLen.ToList();

            var lengths = new List<int> { ptLengths.Count, aadLengths.Count, tagLengths.Count };
            var maxLengthParameter = lengths.Max();
            var ptQueue = new ShuffleQueue<int>(ptLengths);
            var aadQueue = new ShuffleQueue<int>(aadLengths);
            var tagQueue = new ShuffleQueue<int>(tagLengths);

            foreach (var function in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    for (var i = 0; i < maxLengthParameter; i++)
                    {
                        var testGroup = new TestGroup
                        {
                            Function = function,
                            PayloadLength = ptQueue.Pop(),
                            KeyLength = keyLength,
                            AadLength = aadQueue.Pop(),
                            TagLength = tagQueue.Pop(),
                            SaltGen = parameters.SaltGen,
                            IvGeneration = parameters.IvGen,
                            IvGenerationMode = parameters.IvGenMode
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }

            return Task.FromResult(testGroups);
        }

        private List<int> GetTestableValuesFromCapability(MathDomain capability)
        {
            var minMaxDomain = capability.GetDomainMinMaxAsEnumerable();
            var randomCandidates = capability.GetRandomValues(10).ToList();
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
