using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigVer.TestCaseExpectations;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigVer
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestGroupGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            // Use a hash set because the registration allows for duplicate pairings to occur
            // Equality of groups is done via name of the curve and name of the hash function.
            // HashSet eliminates any duplicates that may be registered
            var testGroups = new HashSet<TestGroup>();

            foreach (var curveName in parameters.Curve)
            {
                var curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveName);

                var keyParam = new EddsaKeyParameters
                {
                    Curve = curve,
                    Disposition = EddsaKeyDisposition.None
                };

                if (parameters.Pure)
                {
                    var testGroup = new TestGroup
                    {
                        TestCaseExpectationProvider = new TestCaseExpectationProvider(parameters.IsSample),
                        Curve = curve,
                        PreHash = false
                    };

                    testGroups.Add(testGroup);
                }

                if (parameters.PreHash)
                {
                    var testGroup = new TestGroup
                    {
                        TestCaseExpectationProvider = new TestCaseExpectationProvider(parameters.IsSample),
                        Curve = curve,
                        PreHash = true
                    };

                    testGroups.Add(testGroup);
                }
            }

            return Task.FromResult(testGroups.ToList());
        }
    }
}
