using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SRTP.Tests
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
                var testGroup = new TestGroup
                {
                    AesKeyLength = groupIdx,
                    Kdr = new BitString("ABCD"),
                    TestType = "Sample"
                };

                var tests = new List<TestCase>();
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        MasterKey = new BitString("AB"),
                        MasterSalt = new BitString("ABCD"),
                        Index = new BitString("ABCDEF"),
                        SrtcpIndex = new BitString("ABCDEF12"),
                       
                        SrtpKe = new BitString("1AAADFF1"),
                        SrtcpKa = new BitString("1AAADFF0"),
                        SrtcpKs = new BitString("1AAADFFA"),
                        SrtcpKe = new BitString("1AAADFFB"),
                        SrtpKa = new BitString("1AAADFFC"),
                        SrtpKs = new BitString("1AAADFFC02"),

                        ParentGroup = testGroup,
                        TestCaseId = testId,
                    });
                }

                testGroup.Tests = tests;
                testGroups.Add(testGroup);
            }

            return vectorSet;
        }
    }
}
