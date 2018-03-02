using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.ECC.SigGen.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 1)
        {
            var testGroups = new List<TestGroup>();
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        KeyPair = new EccKeyPair(new EccPoint(1, 2)),
                        Message = new BitString("BEEFFACE"),
                        Signature = new EccSignature(1, 2),
                        TestCaseId = testId
                    });
                }

                testGroups.Add(new TestGroup
                {
                    Curve = Curve.P192,
                    HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                    Tests = tests
                });
            }

            return testGroups;
        }
    }
}
