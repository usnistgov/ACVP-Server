using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.SigGen
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        private IShaFactory _shaFactory = new ShaFactory();

        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet, IEnumerable<TestCase> suppliedResults)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup)g))
            {
                foreach (var test in group.Tests.Select(t => (TestCase)t))
                {
                    list.Add(new TestCaseValidator(test, group, new FfcDsa(_shaFactory.GetShaInstance(group.HashAlg))));
                }
            }

            return list;
        }
    }
}
