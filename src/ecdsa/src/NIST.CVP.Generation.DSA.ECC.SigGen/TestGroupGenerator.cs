using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC.Helpers;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.ECC.SigGen
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters>
    {
        private IShaFactory _shaFactory = new ShaFactory();
        private readonly EccCurveFactory _curveFactory = new EccCurveFactory();

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            // Use a hash set because the registration allows for duplicate pairings to occur
            // Equality of groups is done via name of the curve and name of the hash function.
            // HashSet eliminates any duplicates that may be registered
            var testGroups = new HashSet<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                foreach (var curveName in capability.Curve)
                {
                    var curveEnum = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveName);
                    var curve = _curveFactory.GetCurve(curveEnum);

                    foreach (var hashAlg in capability.HashAlg)
                    {
                        var shaAttributes = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(hashAlg);
                        var sha = new HashFunction(shaAttributes.shaMode, shaAttributes.shaDigestSize);

                        var testGroup = new TestGroup
                        {
                            DomainParameters = new EccDomainParameters(curve),
                            HashAlg = sha,
                            ComponentTest = parameters.ComponentTest
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }

            return testGroups;
        }
    }
}
