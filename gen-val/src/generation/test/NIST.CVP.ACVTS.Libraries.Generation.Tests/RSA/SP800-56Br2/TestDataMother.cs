using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.Sp800_56Br2.DpComponent;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.Sp800_56Br2
{
    public class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, string mode = "sha3", string testType = "aft")
        {
            var tvs = new TestVectorSet
            {
                Algorithm = "RSA",
                Mode = "decryptionPrimitive",
                Revision = "Sp800_56Br2",
                IsSample = true,
            };

            var testGroups = new List<TestGroup>();
            tvs.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Modulo = 2048,
                    TestType = testType,
                    KeyMode = PrivateKeyModes.Standard,
                    TestGroupId = groupIdx
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    var tc = new TestCase
                    {
                        ParentGroup = tg,
                        PlainText = new BitString("ABCD"),
                        CipherText = new BitString("1234"),
                        TestPassed = true,
                        Deferred = true,
                        TestCaseId = testId
                    };
                    tests.Add(tc);
                }
            }

            return tvs;
        }
    }
}
