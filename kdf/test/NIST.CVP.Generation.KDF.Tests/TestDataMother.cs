using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KDF.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, KdfModes kdfMode = KdfModes.Counter, CounterLocations counterLocation = CounterLocations.MiddleFixedData)
        {
            var vectorSet = new TestVectorSet();

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = 
                    new TestGroup
                    {
                        KdfMode = kdfMode,
                        MacMode = MacModes.CMAC_AES128,
                        CounterLocation = counterLocation,
                        CounterLength = 8,
                        KeyOutLength = groupIdx + 24,
                        ZeroLengthIv = false,
                        TestType = "Sample"
                    };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        KeyIn = new BitString("1AAADFFF"),
                        KeyOut = new BitString("7EADDC"),
                        FixedData = new BitString("9998ADCD"),
                        BreakLocation = 3,
                        IV = new BitString("CAFECAFE"),
                        TestCaseId = testId,
                        ParentGroup = tg
                    });
                }
            }
            return vectorSet;
        }
    }
}
