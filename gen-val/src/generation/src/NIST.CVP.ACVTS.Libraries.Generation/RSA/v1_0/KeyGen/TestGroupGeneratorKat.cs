﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.KeyGen
{
    public class TestGroupGeneratorKat : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "KAT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            if (parameters.PubExpMode == PublicExponentModes.Fixed)
            {
                return Task.FromResult(testGroups);
            }

            foreach (var algSpec in parameters.AlgSpecs)
            {
                if (algSpec.RandPQ != PrimeGenFips186_4Modes.B33)
                {
                    continue;
                }

                foreach (var capability in algSpec.Capabilities)
                {
                    foreach (var primeTest in capability.PrimeTests)
                    {
                        // No KATs available for this size
                        if (capability.Modulo == 4096) continue;

                        var testGroup = new TestGroup
                        {
                            PrimeGenMode = algSpec.RandPQ,
                            Modulo = capability.Modulo,
                            PrimeTest = primeTest,
                            PubExp = parameters.PubExpMode,
                            KeyFormat = parameters.KeyFormat,
                            TestType = TEST_TYPE
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }

            return Task.FromResult(testGroups);
        }
    }
}
