﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CBC
{
    public class TestCaseGeneratorNull : ITestCaseGenerator<TestGroup, TestCase>
    {
        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            return Generate(group, null);
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
        {
            return new TestCaseGenerateResponse("This is the null generator -- nothing is generated");
        }

        public int NumberOfTestCasesToGenerate { get; set; }
    }
}