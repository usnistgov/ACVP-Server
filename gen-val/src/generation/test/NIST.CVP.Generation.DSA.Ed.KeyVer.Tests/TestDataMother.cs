﻿using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Math;
using System.Collections.Generic;
using NIST.CVP.Generation.EDDSA.v1_0.KeyVer;

namespace NIST.CVP.Generation.DSA.Ed.KeyVer.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, bool testPassed = true)
        {
            var vectorSet = new TestVectorSet()
            {
                Algorithm = "EDDSA",
                Mode = "KeyVer"
            };

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Curve = Curve.Ed25519
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 5 * groupIdx + 1; testId <= (groupIdx + 1) * 5; testId++)
                {
                    tests.Add(new TestCase
                    {
                        TestCaseId = testId,
                        KeyPair = new EdKeyPair(new BitString("BEEF"), new BitString("FACE")),
                        TestPassed = testPassed,
                        Reason = EddsaKeyDisposition.NotOnCurve,
                        ParentGroup = tg
                    });
                }
            }

            return vectorSet;
        }
    }
}