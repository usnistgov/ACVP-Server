﻿using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_XPN.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups, string direction, string ivGen, string saltGen, bool deferred, bool testPassed)
        {
            var testVectorSet = new TestVectorSet()
            {
                Algorithm = "AES",
                Mode = "XPN",
                IsSample = false
            };

            var testGroups = new List<TestGroup>();
            testVectorSet.TestGroups = testGroups;
            for (int groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    AADLength = 16 + groupIdx * 2,
                    Function = direction,
                    KeyLength = 256 + groupIdx * 2,
                    PTLength = 256 + groupIdx * 2,
                    TagLength = 16 + groupIdx * 2,
                    IVGeneration = ivGen,
                    SaltGen = saltGen,
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
                        Salt = new BitString("CAFECAFE"),
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