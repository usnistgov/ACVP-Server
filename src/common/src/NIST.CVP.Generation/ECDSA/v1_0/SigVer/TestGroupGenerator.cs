﻿using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.ECDSA.v1_0.SigVer.TestCaseExpectations;

namespace NIST.CVP.Generation.ECDSA.v1_0.SigVer
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            // Use a hash set because the registration allows for duplicate pairings to occur
            // Equality of groups is done via name of the curve and name of the hash function.
            // HashSet eliminates any duplicates that may be registered
            var testGroups = new HashSet<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                foreach (var curveName in capability.Curve)
                {
                    foreach (var hashAlg in capability.HashAlg)
                    {
                        var sha = ShaAttributes.GetHashFunctionFromName(hashAlg);
                        var curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveName);

                        var testGroup = new TestGroup
                        {
                            TestCaseExpectationProvider = new TestCaseExpectationProvider(parameters.IsSample),
                            Curve = curve,
                            HashAlg = sha
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }

            return testGroups;
        }
    }
}