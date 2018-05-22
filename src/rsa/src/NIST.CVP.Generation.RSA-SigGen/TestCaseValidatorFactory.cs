using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Signatures;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>
    {
        private readonly IPaddingFactory _paddingFactory;
        private readonly ISignatureBuilder _signatureBuilder;
        private readonly IShaFactory _shaFactory;
        private readonly IRsa _rsa;

        public TestCaseValidatorFactory(IPaddingFactory paddingFactory, ISignatureBuilder sigBuilder, IShaFactory shaFactory, IRsa rsa)
        {
            _paddingFactory = paddingFactory;
            _signatureBuilder = sigBuilder;
            _shaFactory = shaFactory;
            _rsa = rsa;
        }

        public IEnumerable<ITestCaseValidator<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TestGroup, TestCase>>();

            foreach (var group in testVectorSet.TestGroups)
            {
                foreach (var test in group.Tests)
                {
                    if (group.TestType.ToLower() == "gdt")
                    {
                        list.Add(new TestCaseValidatorGDT(test, group, new DeferredTestCaseResolver(_paddingFactory, _signatureBuilder, _shaFactory, _rsa)));
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
