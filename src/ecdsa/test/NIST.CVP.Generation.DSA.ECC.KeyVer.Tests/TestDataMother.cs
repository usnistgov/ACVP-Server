using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Generation.DSA.ECC.KeyVer.Enums;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, bool testPassed = true)
        {
            var vectorSet = new TestVectorSet()
            {
                Algorithm = "ECDSA",
                Mode = "KeyVer"
            };

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Curve = Curve.P224
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 5 * groupIdx + 1; testId <= (groupIdx + 1) * 5; testId++)
                {
                    tests.Add(new TestCase
                    {
                        TestCaseId = testId,
                        KeyPair = new EccKeyPair(new EccPoint(1, 2), 3),
                        TestPassed = testPassed,
                        Reason = TestCaseExpectationEnum.NotOnCurve,
                        ParentGroup = tg
                    });
                }
            }

            return vectorSet;
        }
    }
}
