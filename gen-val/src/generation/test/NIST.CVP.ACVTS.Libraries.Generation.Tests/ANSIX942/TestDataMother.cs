using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX942;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ANSIX942
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(AnsiX942Types kdfType)
        {
            var vectorSet = new TestVectorSet();

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < 1; groupIdx++)
            {
                var domain = new MathDomain().AddSegment(new ValueDomainSegment(groupIdx));
                var tg = new TestGroup
                {
                    KdfType = kdfType,
                    HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d224),
                    ZzLen = domain,
                    OtherInfoLen = domain,
                    KeyLen = domain,
                    SuppInfoLen = domain,
                    TestType = "Sample"
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        Zz = new BitString("ABCD"),
                        OtherInfo = new BitString("ABCDEF"),
                        PartyUInfo = new BitString("ABCDEF"),
                        PartyVInfo = new BitString("ABCDEF"),
                        SuppPubInfo = new BitString("ABCDEF"),
                        SuppPrivInfo = new BitString("ABCDEF"),
                        DerivedKey = new BitString("ABCDEF"),
                        TestCaseId = testId,
                        ParentGroup = tg
                    });
                }
            }

            return vectorSet;
        }
    }
}
