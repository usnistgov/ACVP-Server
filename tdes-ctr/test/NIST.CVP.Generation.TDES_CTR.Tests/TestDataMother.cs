using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_CTR.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 1, string direction = "encrypt")
        {
            var testGroups = new List<TestGroup>();
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        PlainText = new BitString("1AAADFFF"),
                        Deferred = false,
                        CipherText = new BitString("7EADDC"),
                        Key = new BitString("9998ADCD"),
                        Iv = new BitString("CAFECAFE"),
                        TestCaseId = testId
                    });
                }

                testGroups.Add(
                    new TestGroup
                    {
                        Direction = direction,
                        NumberOfKeys = 2,
                        Tests = tests,
                        TestType = "Sample" + groupIdx
                    }
                );
            }
            return testGroups;
        }

        public List<TestGroup> GetCounterTestGroups(int groups = 1, string direction = "encrypt")
        {
            var testGroups = new List<TestGroup>();

            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        Deferred = true,
                        PlainText = new BitString("1AAADFFF"),
                        CipherText = new BitString("7EADDC"),
                        Key = new BitString("9998ADCD"),
                        Ivs = new List<BitString>
                        {
                            new BitString("CAFECAFE"),
                        },
                        TestCaseId = testId
                    });
                }

                testGroups.Add(
                    new TestGroup
                    {
                        Direction = direction,
                        NumberOfKeys = 2,
                        Tests = tests,
                        OverflowCounter = true,
                        IncrementalCounter = true,
                        TestType = "counter"
                    }
                );
            }

            return testGroups;
        }
    }
}
