using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.DRBG.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.DRBG.v1_0
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const int _MAX_BIT_SIZE = 1024;

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var groups = new List<TestGroup>();

            CreateGroups(groups, parameters);

            return Task.FromResult(groups.AsEnumerable());
        }

        private void CreateGroups(List<TestGroup> groups, Parameters parameters)
        {
            foreach (var predResistance in parameters.PredResistanceEnabled)
            {
                foreach (var capability in parameters.Capabilities)
                {
                    var attributes = DrbgAttributesHelper.GetDrbgAttributes(parameters.Algorithm, capability.Mode, capability.DerFuncEnabled);

                    // We don't want to generate test groups that have 1 << 35 sized lengths (as that is the cap in some cases)
                    // Set to a maximum value to generate.
                    capability.EntropyInputLen.SetMaximumAllowedValue(_MAX_BIT_SIZE);
                    capability.NonceLen.SetMaximumAllowedValue(_MAX_BIT_SIZE);
                    capability.PersoStringLen.SetMaximumAllowedValue(_MAX_BIT_SIZE);
                    capability.AdditionalInputLen.SetMaximumAllowedValue(_MAX_BIT_SIZE);

                    var testEntropyInputLens = GetTestableValuesFromCapability(capability.EntropyInputLen.GetDeepCopy());
                    var testNonceLens = GetTestableValuesFromCapability(capability.NonceLen.GetDeepCopy());
                    var testPersoStringLens = GetTestableValuesFromCapability(capability.PersoStringLen.GetDeepCopy());
                    var testAdditionalInputLens = GetTestableValuesFromCapability(capability.AdditionalInputLen.GetDeepCopy());

                    foreach (var entropyLen in testEntropyInputLens)
                    {
                        foreach (var nonceLen in testNonceLens)
                        {
                            foreach (var persoStringLen in testPersoStringLens)
                            {
                                foreach (var additionalInputLen in testAdditionalInputLens)
                                {
                                    var dp = new DrbgParameters
                                    {
                                        Mechanism = attributes.Mechanism,
                                        Mode = attributes.Mode,
                                        SecurityStrength = attributes.MaxSecurityStrength,

                                        DerFuncEnabled = capability.DerFuncEnabled,
                                        PredResistanceEnabled = predResistance,
                                        ReseedImplemented = parameters.ReseedImplemented,

                                        EntropyInputLen = entropyLen,
                                        NonceLen = nonceLen,
                                        PersoStringLen = persoStringLen,
                                        AdditionalInputLen = additionalInputLen,

                                        ReturnedBitsLen = capability.ReturnedBitsLen
                                    };

                                    var tg = new TestGroup
                                    {
                                        DrbgParameters = dp,
                                        TestType = "AFT"
                                    };

                                    groups.Add(tg);
                                }
                            }
                        }
                    }
                }
            }
        }

        private List<int> GetTestableValuesFromCapability(MathDomain capability)
        {
            var minMaxDomain = capability.GetDomainMinMaxAsEnumerable();
            var randomCandidates = capability.GetValues(10).ToList();
            var testValues = new List<int>();
            testValues.AddRangeIfNotNullOrEmpty(minMaxDomain);
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