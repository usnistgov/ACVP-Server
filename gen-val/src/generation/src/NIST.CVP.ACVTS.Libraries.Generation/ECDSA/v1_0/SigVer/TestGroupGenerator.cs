using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.SigVer.TestCaseExpectations;

namespace NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.SigVer
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private readonly bool _randomizeMessagePriorToSign;

        public TestGroupGenerator(bool randomizeMessagePriorToSign)
        {
            _randomizeMessagePriorToSign = randomizeMessagePriorToSign;
        }

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
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
                        var sha = hashAlg.Contains("SHAKE") ? ShaAttributes.GetXofPssHashFunctionFromName(hashAlg) :
                                                                         ShaAttributes.GetHashFunctionFromName(hashAlg);
                        var curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveName);

                        var testGroup = new TestGroup
                        {
                            TestCaseExpectationProvider = new TestCaseExpectationProvider(parameters.IsSample),
                            Curve = curve,
                            HashAlg = sha,
                            Conformance = _randomizeMessagePriorToSign ? "SP800-106" : null,
                            Component = sha.Name.Contains("SHAKE") || parameters.Component
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }

            return Task.FromResult(testGroups.ToList());
        }
    }
}
