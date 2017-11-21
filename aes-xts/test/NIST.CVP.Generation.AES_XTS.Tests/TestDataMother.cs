using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES_XTS;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_XTS.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 1, string direction = "encrypt")
        {
            var testGroups = new List<TestGroup>();
            for (int groupIdx = 0; groupIdx < groups; groupIdx++)
            {

                var tests = new List<ITestCase>();
                for (int testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        PlainText = new BitString("1AAADFFF"),
                        CipherText = new BitString("7EADDC"),
                        Key = new XtsKey(new BitString("9998ADCD")),
                        SequenceNumber = 10,
                        I = new BitString("CAFECAFE"),
                        TestCaseId = testId
                    });
                }

                testGroups.Add(
                    new TestGroup
                    {
                        Direction = direction,
                        KeyLen = 256 + groupIdx * 2,
                        PtLen = 128,
                        TweakMode = "hex",
                        Tests = tests,
                        TestType = "Sample"
                    }
                );
            }
            return testGroups;
        }
    }
}
