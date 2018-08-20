using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.DSA.Ed.SigGen
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
            var groups = BuildTestGroupsAsync(parameters);
            groups.Wait();
            return groups.Result;
        }

        public async Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            // Use a hash set because the registration allows for duplicate pairings to occur
            // Equality of groups is done via name of the curve and name of the hash function.
            // HashSet eliminates any duplicates that may be registered
            var testGroups = new HashSet<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                foreach (var curveName in capability.Curve)
                {
                    var curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveName);

                    EdKeyPair key = null;
                    var param = new EddsaKeyParameters
                    {
                        Curve = curve
                    };

                    if (parameters.IsSample)
                    {
                        var keyResult = await _oracle.GetEddsaKeyAsync(param);
                        key = keyResult.Key;
                    }

                    if (parameters.Pure)
                    {
                        var testGroup = new TestGroup
                        {
                            Curve = curve,
                            PreHash = false,
                            KeyPair = key
                        };
                        testGroups.Add(testGroup);
                    }

                    if (parameters.PreHash)
                    {
                        var testGroup = new TestGroup
                        {
                            Curve = curve,
                            PreHash = true,
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
