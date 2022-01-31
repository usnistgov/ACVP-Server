using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v2_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.XTS.v2_0
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups, bool ptLenMatch, BlockCipherDirections direction, XtsTweakModes tweakMode)
        {
            var testVectorSet = new TestVectorSet()
            {
                Algorithm = "AES",
                Mode = "XTS",
                Revision = "2.0",
                IsSample = false
            };

            var testGroups = new List<TestGroup>();
            testVectorSet.TestGroups = testGroups;
            for (int groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Direction = direction,
                    KeyLen = 128,
                    PayloadLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    DataUnitLen = ptLenMatch ? null : new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    TweakMode = tweakMode,
                    TestType = "AFT"
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (int testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    var tc = new TestCase
                    {
                        PlainText = new BitString("1AAADFFF"),
                        CipherText = new BitString("7EADDC"),
                        Key = new BitString("9998ADCD"),
                        TestCaseId = testId,
                        ParentGroup = tg,
                        DataUnitLen = 128,
                        // note not actually true in real scenarios, 
                        // just want to be able to confirm the property isn't serialized.
                        Deferred = true
                    };

                    switch (tweakMode)
                    {
                        case XtsTweakModes.Hex:
                            tc.I = new BitString("CAFECAFE");
                            break;
                        case XtsTweakModes.Number:
                            tc.SequenceNumber = 10;
                            break;
                    }

                    tests.Add(tc);
                }
            }
            return testVectorSet;
        }
    }
}
