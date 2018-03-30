using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KDF.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 1, string kdfMode = "counter", string counterLocation = "middle fixed data")
        {
            var testGroups = new List<TestGroup>();
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        KeyIn = new BitString("1AAADFFF"),
                        Deferred = true,
                        KeyOut = new BitString("7EADDC"),
                        FixedData = new BitString("9998ADCD"),
                        BreakLocation = 3,
                        IV = new BitString("CAFECAFE"),
                        TestCaseId = testId
                    });
                }

                testGroups.Add(
                    new TestGroup
                    {
                        KdfMode = EnumHelpers.GetEnumFromEnumDescription<KdfModes>(kdfMode),
                        MacMode = MacModes.CMAC_AES128,
                        CounterLocation = EnumHelpers.GetEnumFromEnumDescription<CounterLocations>(counterLocation),
                        CounterLength = 8,
                        KeyOutLength = groupIdx + 24,
                        ZeroLengthIv = false,
                        Tests = tests,
                        TestType = "Sample"
                    }
                );
            }
            return testGroups;
        }
    }
}
