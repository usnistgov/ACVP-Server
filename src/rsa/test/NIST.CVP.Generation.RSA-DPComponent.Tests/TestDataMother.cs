using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_DPComponent.Tests
{
    public class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, string mode = "sha3", string testType = "aft")
        {
            var tvs = new TestVectorSet
            {
                Algorithm = "RSA",
                Mode = "DecryptionPrimitiveComponent",
                IsSample = true,
            };

            var testGroups = new List<TestGroup>();
            tvs.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Modulo = 2048,
                    TotalFailingCases = 2,
                    TotalTestCases = 6,
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
                        ResultsArray = new List<AlgoArrayResponseSignature>
                        {
                            new AlgoArrayResponseSignature()
                            {
                                PlainText = new BitString("ABCD"),
                                CipherText = new BitString("1234"),
                                Key = new KeyPair() { PrivKey = new PrivateKey {D = 1, P = 2, Q = 3}, PubKey = new PublicKey {E = 4, N = 5}},
                                FailureTest = true,
                            }
                        },
                        Deferred = true,
                        TestCaseId = testId
                    };
                    tests.Add(tc);
                }
            }

            return tvs;
        }
    }
}
