﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_SPComponent
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet, IEnumerable<TestCase> suppliedResults)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup) g))
            {
                foreach (var test in group.Tests.Select(t => (TestCase) t))
                {
                    list.Add(new TestCaseValidator(test));
                }
            }

            return list;
        }
    }
}