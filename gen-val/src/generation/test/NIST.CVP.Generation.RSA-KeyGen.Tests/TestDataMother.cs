﻿using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;
using System.Collections.Generic;
using NIST.CVP.Generation.RSA.v1_0.KeyGen;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    public class TestDataMother
    {
        public static TestVectorSet GetTestGroups(PrimeGenFips186_4Modes primeGenMode)
        {
            var tvs = new TestVectorSet
            {
                Algorithm = "RSA",
                Mode = "KeyGen",
                IsSample = true,
            };

            var testGroups = new List<TestGroup>();
            tvs.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < 1; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Modulo = 2048,
                    HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                    PrimeGenMode = primeGenMode,
                    PubExp = PublicExponentModes.Fixed,
                    FixedPubExp = new BitString("ABCD"),
                    KeyFormat = PrivateKeyModes.Crt,
                    InfoGeneratedByServer = true,
                    PrimeTest = PrimeTestFips186_4Modes.TblC2,
                    TestType = "aft"
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    var tc = new TestCase
                    {
                        Key = new KeyPair
                        {
                            PrivKey = new CrtPrivateKey
                            {
                                DMP1 = 1,
                                DMQ1 = 2,
                                IQMP = 3,
                                P = 4,
                                Q = 5
                            },
                            PubKey = new PublicKey
                            {
                                E = 6,
                                N = 7
                            }
                        },
                        Bitlens = new [] {32, 32, 32, 32},
                        Deferred = false,
                        Seed = new BitString("ABCDEF"),
                        XP = new BitString("BEEFFACE"),
                        XP1 = new BitString("BEEFFACE"),
                        XP2 = new BitString("BEEFFACE"),
                        XQ = new BitString("BEEFFACE"),
                        XQ1 = new BitString("BEEFFACE"),
                        XQ2 = new BitString("BEEFFACE"),
                        ParentGroup = tg,
                        TestCaseId = testId
                    };
                    tests.Add(tc);
                }
            }

            return tvs;
        }
    }
}