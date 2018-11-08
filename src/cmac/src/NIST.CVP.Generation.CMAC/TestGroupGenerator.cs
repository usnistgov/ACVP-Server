using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.CMAC
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            return CreateGroups(parameters, testGroups);
        }

        private IEnumerable<TestGroup> CreateGroups(Parameters parameters, List<TestGroup> testGroups)
        {
            foreach (var capability in parameters.Capabilities)
            {
                int[] msgLens = null;
                int[] macLens = null;

                DetermineLengths(capability, ref msgLens, ref macLens);

                foreach (var direction in capability.Direction)
                {
                    if (parameters.Algorithm.Contains("aes", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (var keyLen in capability.KeyLen)
                        {
                            foreach (var msgLen in msgLens)
                            {
                                foreach (var macLen in macLens)
                                {
                                    if (!AlgorithmSpecificationMapping.Map
                                        .TryFirst(
                                            w => w.algoSpecification.Equals(parameters.Algorithm, StringComparison.OrdinalIgnoreCase) ||
                                                 (w.algoSpecification.StartsWith(parameters.Algorithm, StringComparison.OrdinalIgnoreCase) &&
                                                  w.keySize == keyLen),
                                            out var result))
                                    {
                                        throw new ArgumentException("Invalid Algorithm provided.");
                                    }

                                    var tg = new TestGroup
                                    {
                                        CmacType = result.mappedCmacType,
                                        Function = direction,
                                        KeyLength = keyLen,
                                        MessageLength = msgLen,
                                        MacLength = macLen,
                                    };

                                    testGroups.Add(tg);
                                }
                            }
                        }
                    }

                    if (parameters.Algorithm.Contains("tdes", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (var keyingOption in capability.KeyingOption)
                        {
                            foreach (var msgLen in msgLens)
                            {
                                foreach (var macLen in macLens)
                                {
                                    if (!AlgorithmSpecificationMapping.Map
                                        .TryFirst(
                                            w => w.algoSpecification.Equals(parameters.Algorithm, StringComparison.OrdinalIgnoreCase) ||
                                                 w.algoSpecification.StartsWith(parameters.Algorithm, StringComparison.OrdinalIgnoreCase),
                                            out var result))
                                    {
                                        throw new ArgumentException("Invalid Algorithm provided.");
                                    }

                                    if (keyingOption == 2 && direction.Equals("gen", StringComparison.OrdinalIgnoreCase))
                                    {
                                        continue;
                                    }

                                    var tg = new TestGroup
                                    {
                                        CmacType = result.mappedCmacType,
                                        Function = direction,
                                        KeyingOption = keyingOption,
                                        MessageLength = msgLen,
                                        MacLength = macLen,
                                    };

                                    testGroups.Add(tg);
                                }
                            }
                        }
                    }
                }
            }

            return testGroups;
        }

        private void DetermineLengths(Capability capability, ref int[] msgLens, ref int[] macLens)
        {
            // Set MathDomain options to get randomly from the potential sequence
            capability.MsgLen.SetRangeOptions(RangeDomainSegmentOptions.Random);
            capability.MacLen.SetRangeOptions(RangeDomainSegmentOptions.Random);

            // Get the min/max of the sequences
            var msgMinMax = capability.MsgLen.GetDomainMinMaxAsEnumerable();
            var macMinMax = capability.MacLen.GetDomainMinMaxAsEnumerable();

            // Set the "max value" of the ranges to ensure the random value is on the lower end
            capability.MsgLen.SetMaximumAllowedValue(1024 * 10);
            capability.MacLen.SetMaximumAllowedValue(1024 * 10);

            // Get random lengths to test with, ensuring not equal to the min/max already grabbed.
            var randomMsgLengths = capability.MsgLen.GetValues(3).Where(w => !msgMinMax.Contains(w)).Take(1);
            var randomMacLengths = capability.MacLen.GetValues(3).Where(w => !macMinMax.Contains(w)).Take(1);

            List<int> msgLensList = new List<int>();
            msgLensList.AddRangeIfNotNullOrEmpty(msgMinMax);
            msgLensList.AddRangeIfNotNullOrEmpty(randomMsgLengths);

            List<int> macLensList = new List<int>();
            macLensList.AddRangeIfNotNullOrEmpty(macMinMax);
            macLensList.AddRangeIfNotNullOrEmpty(randomMacLengths);

            msgLens = msgLensList.OrderBy(ob => ob).ToArray();
            macLens = macLensList.OrderBy(ob => ob).ToArray();
        }
    }
}