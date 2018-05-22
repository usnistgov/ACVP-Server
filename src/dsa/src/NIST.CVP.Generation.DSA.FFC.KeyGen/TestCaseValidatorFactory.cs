using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>
    {
        private readonly IDsaFfcFactory _dsaFactory;

        public TestCaseValidatorFactory(IDsaFfcFactory dsaFactory)
        {
            _dsaFactory = dsaFactory;
        }

        public IEnumerable<ITestCaseValidator<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TestGroup, TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => g))
            {
                foreach (var test in group.Tests.Select(t => t))
                {
                    var deferredResolver = new DeferredTestCaseResolver(_dsaFactory);
                    list.Add(new TestCaseValidator(test, group, deferredResolver));
                }
            }

            return list;
        }
    }
}
