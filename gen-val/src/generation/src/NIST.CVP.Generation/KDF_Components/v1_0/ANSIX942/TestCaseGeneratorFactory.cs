﻿using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.KDF_Components.v1_0.ANSIX942
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactoryAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestCaseGeneratorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public ITestCaseGeneratorAsync<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            return new TestCaseGenerator(_oracle);
        }
    }
}