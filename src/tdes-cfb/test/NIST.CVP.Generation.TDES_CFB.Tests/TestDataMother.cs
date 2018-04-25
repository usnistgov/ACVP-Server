﻿using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Generation.TDES_CFB.Tests
{
    public class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, string direction = "encrypt", string testType = "aft")
        {
            var tvs = new TestVectorSet
            {
                Algorithm = "TDES",
                IsSample = true,
                Mode = "CFB"
            };

            var testGroups = new List<TestGroup>();
            tvs.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Function = direction,
                    KeyingOption = groupIdx + 1,
                    TestType = testType
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    var tc = new TestCase
                    {
                        ParentGroup = tg,
                        PlainText = new BitString("1AAADFFF"),                       
                        Deferred = true,
                        CipherText = new BitString("7EADDC"),
                        Key1 = new BitString("9998ADCD9998ADCD"),
                        Key2 = new BitString("9998ADCD9998ADCD"),
                        Key3 = new BitString("9998ADCD9998ADCD"),
                        Iv = new BitString("CAFECAFE"),
                        TestCaseId = testId
                    };
                    tests.Add(tc);

                    if (testType.Equals("mct", StringComparison.OrdinalIgnoreCase))
                    {
                        tc.ResultsArray = new List<AlgoArrayResponse>
                        {
                            new AlgoArrayResponse
                            {
                                PlainText = new BitString("FF1AAADFFF"),
                                CipherText = new BitString("FF7EADDC"),
                                Key1 = new BitString("9998ADCD9998ADCD"),
                                Key2 = new BitString("9998ADCD9998ADCD"),
                                Key3 = new BitString("9998ADCD9998ADCD"),
                                IV = new BitString("FFCAFECAFE"),
                            }
                        };
                    }
                }
            }

            return tvs;
        }
    }
}
