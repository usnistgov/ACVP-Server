﻿using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.TupleHash;
using NIST.CVP.ACVTS.Libraries.Generation.TupleHash.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TupleHash
{
    public class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, string mode = "", string testType = "aft", bool hexCustomization = false)
        {
            var tvs = new TestVectorSet
            {
                Algorithm = "TupleHash",
                IsSample = true,
            };

            var testGroups = new List<TestGroup>();
            tvs.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Function = mode,
                    DigestSize = groupIdx + 1,
                    MessageLength = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    OutputLength = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    TestType = testType,
                    HexCustomization = hexCustomization,
                    XOF = true
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    var tc = new TestCase
                    {
                        ParentGroup = tg,
                        Tuple = new List<BitString>(new BitString[] { new BitString("ABCD") }),
                        Digest = new BitString("ABCDEF"),
                        Customization = "custom",
                        CustomizationHex = new BitString(8),
                        Deferred = true,
                        TestCaseId = testId
                    };
                    tests.Add(tc);

                    if (testType.Equals("mct", StringComparison.OrdinalIgnoreCase))
                    {
                        tc.ResultsArray = new List<AlgoArrayResponse>
                        {
                            new AlgoArrayResponse
                            {
                                Tuple = new List<BitString>(new BitString[] { new BitString("123456") }),
                                Digest = new BitString("987654")
                            }
                        };
                    }
                }
            }

            return tvs;
        }
    }
}
