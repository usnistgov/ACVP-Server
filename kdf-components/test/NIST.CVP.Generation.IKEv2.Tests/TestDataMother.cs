using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.IKEv2.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 1)
        {
            var testGroups = new List<TestGroup>();
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        SpiInit = new BitString("1AAADFF1"),
                        SpiResp = new BitString("1AAADFF0"),
                        NResp = new BitString("1AAADFFA"),
                        NInit = new BitString("1AAADFFB"),
                        Gir = new BitString("1AAADFFC"),
                        GirNew = new BitString("1AAADFFC02"),
                        SKeySeed = new BitString("1AAADFFD"),
                        DerivedKeyingMaterial = new BitString("1AAADFFE"),
                        DerivedKeyingMaterialChild = new BitString("7EADDCAB"),
                        DerivedKeyingMaterialDh = new BitString("7EADDC"),
                        SKeySeedReKey = new BitString("9998ADCD"),
                        TestCaseId = testId
                    });
                }

                testGroups.Add(
                    new TestGroup
                    {
                        HashAlg = new HashFunction(ModeValues.SHA1, DigestSizes.d160),
                        DerivedKeyingMaterialLength = groupIdx + 64,
                        NInitLength = groupIdx + 64,
                        NRespLength = groupIdx + 64,
                        GirLength = groupIdx + 64,
                        Tests = tests,
                        TestType = "Sample"
                    }
                );
            }
            return testGroups;
        }
    }
}
