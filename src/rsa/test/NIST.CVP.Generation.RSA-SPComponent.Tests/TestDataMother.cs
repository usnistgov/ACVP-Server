using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Math;
using System.Collections.Generic;
using NIST.CVP.Generation.RSA.v1_0.SpComponent;

namespace NIST.CVP.Generation.RSA_SPComponent.Tests
{
    public class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, PrivateKeyModes keyFormat = PrivateKeyModes.Standard)
        {
            var tvs = new TestVectorSet
            {
                Algorithm = "RSA",
                Mode = "SignaturePrimitiveComponent",
                IsSample = true,
            };

            var testGroups = new List<TestGroup>();
            tvs.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Modulo = 2048,
                    KeyFormat = keyFormat,
                    TestType = "aft"
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    var tc = new TestCase
                    {
                        ParentGroup = tg,
                        Message = new BitString("ABCD"),
                        Signature = new BitString("1234"),
                        Key = GetKey(keyFormat),
                        TestPassed = testId % 2 == 0,
                        Deferred = false,
                        TestCaseId = testId
                    };
                    tests.Add(tc);
                }
            }

            return tvs;
        }

        private static KeyPair GetKey(PrivateKeyModes keyFormat)
        {
            var pubKey = new PublicKey
            {
                E = 1,
                N = 2
            };

            if (keyFormat == PrivateKeyModes.Standard)
            {
                var key = new KeyPair
                {
                    PubKey = pubKey,
                    PrivKey = new PrivateKey
                    {
                        D = 3,
                        P = 4,
                        Q = 5
                    }
                };

                return key;
            }
            else
            {
                var key = new KeyPair
                {
                    PubKey = pubKey,
                    PrivKey = new CrtPrivateKey
                    {
                        DMP1 = 3,
                        DMQ1 = 4,
                        IQMP = 5,
                        P = 6,
                        Q = 7
                    }
                };

                return key;
            }
        }
    }
}
