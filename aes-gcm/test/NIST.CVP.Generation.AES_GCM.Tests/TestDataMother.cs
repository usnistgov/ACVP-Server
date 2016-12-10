using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_GCM.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 1, string direction = "encrypt", bool failureTest = false)
        {
            var testGroups = new List<TestGroup>();
            for (int groupIdx = 0; groupIdx < groups; groupIdx++)
            {

                var tests = new List<ITestCase>();
                for (int testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        AAD = new BitString("0AAD"),
                        PlainText = new BitString("1AAADFFF"),
                        Deferred = false,
                        FailureTest = failureTest,
                        Tag = new BitString("FFFA4444"),
                        CipherText = new BitString("7EADDC"),
                        Key = new BitString("9998ADCD"),
                        TestCaseId = testId
                    });
                }

                testGroups.Add(
                    new TestGroup
                    {
                        AADLength = 16 + groupIdx * 2,
                        Function = direction,
                        IVGeneration = "blah",
                        IVGenerationMode = "internal",
                        IVLength = 96 + groupIdx * 2,
                        KeyLength = 256 + groupIdx * 2,
                        PTLength = 256 + groupIdx * 2,
                        TagLength = 16 + groupIdx * 2,
                        Tests = tests
                    }
                );
            }
            return testGroups;
        }
    }
}
