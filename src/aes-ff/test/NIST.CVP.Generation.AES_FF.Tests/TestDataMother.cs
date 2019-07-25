using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.AES_FFX.v1_0.Base;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using System.Collections.Generic;

namespace NIST.CVP.Generation.AES_FF.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, BlockCipherDirections direction = BlockCipherDirections.Decrypt, string testType = "aft")
        {
            var tvs = new TestVectorSet()
            {
                Algorithm = "ACVP-AES-FF3-1",
                IsSample = true
            };

            var testGroups = new List<TestGroup>();
            tvs.TestGroups = testGroups;
            for (int groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {

                    Function = direction,
                    KeyLength = 256 + groupIdx * 2,
                    TestType = testType,
                    AlgoMode = AlgoMode.AES_FF3_1_v1_0,
                    TweakLen = new MathDomain().AddSegment(new ValueDomainSegment(56)),
                    Capability = new Capability()
                    {
                        Radix = 10,
                        Alphabet = "0123456789",
                        MinLen = 10,
                        MaxLen = 26
                    }
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (int testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    var tc = new TestCase
                    {
                        ParentGroup = tg,
                        PlainText = "abcdef",
                        CipherText = "abcdef",
                        Key = new BitString("9998ADCD"),
                        Tweak = new BitString("CAFECAFE"),
                        TestCaseId = testId
                    };
                    tests.Add(tc);
                }
            }

            return tvs;
        }
    }
}
