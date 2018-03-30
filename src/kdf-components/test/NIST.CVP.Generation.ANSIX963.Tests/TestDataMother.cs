using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.ANSIX963.Tests
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
                        Z = new BitString("AB"),
                        SharedInfo = new BitString("ABCD"),
                        KeyData = new BitString("ABCDEF"),
                        TestCaseId = testId,
                    });
                }

                testGroups.Add(
                    new TestGroup
                    {
                        FieldSize = groupIdx,
                        HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d224),
                        SharedInfoLength = groupIdx,
                        KeyDataLength = groupIdx,
                        Tests = tests,
                        TestType = "Sample"
                    }
                );
            }

            return testGroups;
        }
    }
}
