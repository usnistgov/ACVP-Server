﻿using NIST.CVP.Math;
using System.Collections.Generic;
using NIST.CVP.Generation.KDF_Components.v1_0.TPMv1_2;

namespace NIST.CVP.Generation.TPM.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroup()
        {
            var vectorSet = new TestVectorSet();

            var testGroup = new TestGroup
            {
                TestType = "aft",
                TestGroupId = 1
            };

            var tests = new List<TestCase>();
            testGroup.Tests = tests;
            
            for (var i = 0; i < 15; i++)
            {
                tests.Add(new TestCase
                {
                    Auth = new BitString("1234"),
                    NonceEven = new BitString("5678"),
                    NonceOdd = new BitString("9ABC"),
                    SKey = new BitString("DEF0"),
                    TestCaseId = i,
                    ParentGroup = testGroup
                });
            }

            vectorSet.TestGroups = new List<TestGroup>
            {
                testGroup
            };

            return vectorSet;
        }
    }
}
