using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Crypto.DRBG;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DRBG.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1)
        {
            var testVectorSet = new TestVectorSet();

            List<TestGroup> list = new EditableList<TestGroup>();
            testVectorSet.TestGroups = list;

            for (int i = 0; i < groups; i++)
            {
                var tg = new TestGroup
                {
                    DrbgParameters = new DrbgParameters()
                    {
                        NonceLen = 1,
                        AdditionalInputLen = 2,
                        EntropyInputLen = 3,
                        ReturnedBitsLen = 4,
                        PersoStringLen = 5,
                        DerFuncEnabled = true,
                        Mode = DrbgMode.AES128,
                        SecurityStrength = 128,
                        PredResistanceEnabled = true,
                        ReseedImplemented = true,
                        Mechanism = DrbgMechanism.Counter
                    },
                    TestType = "AFT"
                };
                list.Add(tg);

                var tests = new List<TestCase>()
                {
                    new TestCase()
                    {
                        EntropyInput = new BitString("11"),
                        Nonce = new BitString("22"),
                        PersoString = new BitString("33"),
                        OtherInput = new List<OtherInput>()
                        {
                            new OtherInput()
                            {
                                AdditionalInput = new BitString("44"),
                                EntropyInput = new BitString("55")
                            }
                        },
                        ReturnedBits = new BitString("42"),
                        ParentGroup = tg
                    }
                };
                tg.Tests = tests;
            }

            return testVectorSet;
        }

        public static TestVectorSet GetSampleVectorSet(
            int numGroups = 1,
            int numTestCases = 1,
            bool reseed = false,
            bool predictionResistance = false
        )
        {
            TestVectorSet testVectorSet = new TestVectorSet()
            {
                Algorithm = "drbg",
                Mode = "ctr"
            };

            List<TestGroup> groups = new List<TestGroup>();
            testVectorSet.TestGroups = groups;

            for (int groupI = 0; groupI < numGroups; groupI++)
            {
                var group = new TestGroup()
                {
                    AdditionalInputLen = 42,
                    DerFunc = true,
                    PersoStringLen = 50,
                    EntropyInputLen = 64,
                    NonceLen = 55,
                    PredResistance = predictionResistance,
                    ReSeed = reseed
                };

                groups.Add(group);

                List<TestCase> tests = new List<TestCase>();
                group.Tests = tests;
                for (int testI = 0; testI < numTestCases; testI++)
                {
                    var test = new TestCase()
                    {
                        ParentGroup = group
                    };

                    tests.Add(test);
                }
            }

            return testVectorSet;
        }
    }
}