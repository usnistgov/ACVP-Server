using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDF.SP800_108r1.KMAC;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KDF.KMAC
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(bool nullLabel = false)
        {
            var vectorSet = new TestVectorSet();

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < 2; groupIdx++)
            {
                var tg = new TestGroup
                {
                    TestGroupId = groupIdx,
                    MacMode = MacModes.KMAC_128,
                    ContextLength = new MathDomain(),
                    DerivedKeyLength = new MathDomain(),
                    KeyDerivationKeyLength = new MathDomain(),
                    LabelLength = nullLabel ? null : new MathDomain(),
                    TestType = "Sample"
                };
                
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        TestCaseId = testId,
                        Context = new BitString("ABCD"),
                        DerivedKey = new BitString("5678"),
                        DerivedKeyLength = 16,
                        KeyDerivationKey = new BitString("1234"),
                        Label = nullLabel ? null : new BitString("90EF"),
                        ParentGroup = tg
                    });
                }
            }

            return vectorSet;
        }
    }
}
