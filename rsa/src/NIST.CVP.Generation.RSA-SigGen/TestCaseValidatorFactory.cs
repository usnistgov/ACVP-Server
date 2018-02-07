using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Signatures;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        private readonly IPaddingFactory _paddingFactory;
        private readonly ISignatureBuilder _signatureBuilder;
        private readonly IShaFactory _shaFactory;

        public TestCaseValidatorFactory(IPaddingFactory paddingFactory, ISignatureBuilder sigBuilder, IShaFactory shaFactory)
        {
            _paddingFactory = paddingFactory;
            _signatureBuilder = sigBuilder;
            _shaFactory = shaFactory;
        }

        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet, IEnumerable<TestCase> suppliedResults)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup) g))
            {
                foreach (var test in group.Tests.Select(t => (TestCase) t))
                {
                    if (group.TestType.ToLower() == "gdt")
                    {
                        list.Add(new TestCaseValidatorGDT(test, group, new DeferredTestCaseResolver(_paddingFactory, _signatureBuilder, _shaFactory)));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorNull($"Could not determine TestType for TestCase", test.TestCaseId));
                    }
                }
            }

            return list;
        }
    }
}
