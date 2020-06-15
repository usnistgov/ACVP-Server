using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KDF_Components.v1_0.ANXIX963;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.ANSIX963.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1)
        {
            var vectorSet = new TestVectorSet();

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    FieldSize = groupIdx,
                    HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d224),
                    SharedInfoLength = groupIdx,
                    KeyDataLength = groupIdx,
                    TestType = "Sample"
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        Z = new BitString("AB"),
                        SharedSecret = new BitString("AB").ToPositiveBigInteger(),
                        SharedInfo = new BitString("ABCD"),
                        KeyData = new BitString("ABCDEF"),
                        TestCaseId = testId,
                        ParentGroup = tg
                    });
                }
            }

            return vectorSet;
        }
    }
}
