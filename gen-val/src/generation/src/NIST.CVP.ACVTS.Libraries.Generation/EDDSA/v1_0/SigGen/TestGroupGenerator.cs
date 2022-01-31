using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigGen
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";

        private readonly IOracle _oracle;

        public TestGroupGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            // Use a hash set because the registration allows for duplicate pairings to occur
            // Equality of groups is done via name of the curve and name of the hash function.
            // HashSet eliminates any duplicates that may be registered

            var testGroups = new HashSet<TestGroup>();

            if (!parameters.IsSample)
            {
                foreach (var curveName in parameters.Curve)
                {
                    var curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveName);

                    if (parameters.Pure)
                    {
                        var testGroup = new TestGroup
                        {
                            Curve = curve,
                            PreHash = false,
                            TestType = TEST_TYPE
                        };
                        testGroups.Add(testGroup);
                    }

                    if (parameters.PreHash)
                    {
                        var testGroup = new TestGroup
                        {
                            Curve = curve,
                            PreHash = true,
                            TestType = TEST_TYPE
                        };
                        testGroups.Add(testGroup);
                    }
                }

                return testGroups.ToList();
            }

            // Generate keys upfront for sample
            Dictionary<TestGroup, Task<EddsaKeyResult>> map = new Dictionary<TestGroup, Task<EddsaKeyResult>>();

            foreach (var curveName in parameters.Curve)
            {
                var curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveName);

                var param = new EddsaKeyParameters
                {
                    Curve = curve
                };

                if (parameters.Pure)
                {
                    var testGroup = new TestGroup
                    {
                        Curve = curve,
                        PreHash = false,
                        TestType = TEST_TYPE
                    };
                    map.Add(testGroup, _oracle.GetEddsaKeyAsync(param));
                }

                if (parameters.PreHash)
                {
                    var testGroup = new TestGroup
                    {
                        Curve = curve,
                        PreHash = true,
                        TestType = TEST_TYPE
                    };
                    map.Add(testGroup, _oracle.GetEddsaKeyAsync(param));
                }
            }

            await Task.WhenAll(map.Values);

            foreach (var keyValuePair in map)
            {
                var group = keyValuePair.Key;
                var keyPair = keyValuePair.Value.Result;
                group.KeyPair = keyPair.Key;

                testGroups.Add(group);
            }

            return testGroups.ToList();
        }
    }
}
