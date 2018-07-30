using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.DSA.ECC.SigGen
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestGroupGenerator(IOracle oracle)
        {
            _oracle = oracle;
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

                        EccKeyPair key = null;
                        var param = new EcdsaKeyParameters
                        {
                            Curve = curve
                        };

                        if (parameters.IsSample)
                        {
                            key = _oracle.GetEcdsaKey(param).Key;
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
