using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup)g))
            {
                foreach (var test in group.Tests.Select(t => (TestCase)t))
                {
                    list.Add(new TestCaseValidator(test, group, new EccDsa(EntropyProviderTypes.Testable)));
                }
            }

            return list;
        }
    }
}
