using System;
using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.ECC.SigGen
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private readonly IDsaEccFactory _eccDsaFactory;
        private readonly IEccCurveFactory _curveFactory;

        public TestGroupGenerator(IDsaEccFactory eccDsaFactory, IEccCurveFactory curveFactory)
        {
            _eccDsaFactory = eccDsaFactory;
            _curveFactory = curveFactory;
        }

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

                        // Generate the key
                        EccKeyPair key = null;
                        if (parameters.IsSample)
                        {
                            var eccDsa = _eccDsaFactory.GetInstance(sha);
                            var domainParams = new EccDomainParameters(_curveFactory.GetCurve(curve));
                            var keyResult = eccDsa.GenerateKeyPair(domainParams);
                            key = keyResult.KeyPair;
                        }

                        var testGroup = new TestGroup
                        {
                            Curve = curve,
                            HashAlg = sha,
                            ComponentTest = parameters.ComponentTest,
                            KeyPair = key
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }

            return testGroups;
        }
    }
}
