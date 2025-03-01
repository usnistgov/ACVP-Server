﻿using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.AES_GCM.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.GCM
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups, string direction, string ivGen, bool deferred, bool testPassed)
        {
            var testVectorSet = new TestVectorSet()
            {
                Algorithm = "AES",
                Mode = "GCM",
                IsSample = false
            };

            var testGroups = new List<TestGroup>();
            testVectorSet.TestGroups = testGroups;
            for (int groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    AlgoMode = AlgoMode.AES_GCM_v1_0,
                    AadLength = 16 + groupIdx * 2,
                    Function = direction,
                    IvGeneration = ivGen,
                    IvLength = 96 + groupIdx * 2,
                    KeyLength = 256 + groupIdx * 2,
                    PayloadLength = 256 + groupIdx * 2,
                    TagLength = 16 + groupIdx * 2,
                    TestType = "AFT"
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (int testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    var tc = new TestCase
                    {
                        IV = new BitString("00FF00FF"),
                        AAD = new BitString("0AAD"),
                        PlainText = new BitString("1AAADFFF"),
                        Deferred = deferred,
                        TestPassed = testPassed,
                        CipherText = new BitString("7EADDC"),
                        Tag = new BitString("1237AB"),
                        Key = new BitString("9998ADCD"),
                        TestCaseId = testId,
                        ParentGroup = tg
                    };
                    tests.Add(tc);
                }
            }
            return testVectorSet;
        }
    }
}
