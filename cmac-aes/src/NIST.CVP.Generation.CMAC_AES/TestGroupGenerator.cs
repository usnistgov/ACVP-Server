using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.CMAC_AES
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters>
    {
        public int[] MsgLens { get; private set; }
        public int[] MacLens { get; private set; }

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<ITestGroup>();

            return CreateGroups(parameters, testGroups);
        }

        private IEnumerable<ITestGroup> CreateGroups(Parameters parameters, List<ITestGroup> testGroups)
        {
            if (!AlgorithmSpecificationMapping.Map
                .TryFirst(
                    w => w.algoSpecification.Equals(parameters.Algorithm, StringComparison.OrdinalIgnoreCase),
                    out var result))
            {
                throw new ArgumentException("Invalid Algorithm provided.");
            }

            DetermineLengths(parameters);

            foreach (var function in parameters.Direction)
            {
                foreach (var msgLen in MsgLens)
                {
                    foreach (var macLen in MacLens)
                    {
                        TestGroup tg = new TestGroup()
                        {
                            CmacType = result.mappedCmacType,
                            Function = function,
                            KeyLength = result.keySize,
                            MessageLength = msgLen,
                            MacLength = macLen
                        };

                        testGroups.Add(tg);
                    }
                }
            }

            return testGroups;
        }

        private void DetermineLengths(Parameters parameters)
        {
            // Set MathDomain options to get randomly from the potential sequence
            parameters.MsgLen.SetRangeOptions(RangeDomainSegmentOptions.Random);
            parameters.MacLen.SetRangeOptions(RangeDomainSegmentOptions.Random);

            // Get the min/max of the sequences
            var msgMinMax = parameters.MsgLen.GetDomainMinMaxAsEnumerable();
            var macMinMax = parameters.MacLen.GetDomainMinMaxAsEnumerable();

            // Set the "max value" of the ranges to ensure the random value is on the lower end
            parameters.MsgLen.SetMaximumAllowedValue(1024 * 10);
            parameters.MacLen.SetMaximumAllowedValue(1024 * 10);

            // Get random lengths to test with, ensuring not equal to the min/max already grabbed.
            var randomMsgLengths = parameters.MsgLen.GetValues(3).Where(w => !msgMinMax.Contains(w)).Take(1);
            var randomMacLengths = parameters.MacLen.GetValues(3).Where(w => !macMinMax.Contains(w)).Take(1);

            List<int> msgLens = new List<int>();
            msgLens.AddRangeIfNotNullOrEmpty(msgMinMax);
            msgLens.AddRangeIfNotNullOrEmpty(randomMsgLengths);

            List<int> macLens = new List<int>();
            macLens.AddRangeIfNotNullOrEmpty(macMinMax);
            macLens.AddRangeIfNotNullOrEmpty(randomMacLengths);

            MsgLens = msgLens.OrderBy(ob => ob).ToArray();
            MacLens = macLens.OrderBy(ob => ob).ToArray();
        }
    }
}