using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        private readonly IKeyBuilder _keyBuilder;
        private readonly IKeyComposerFactory _keyComposerFactory;
        private readonly IShaFactory _shaFactory;

        public TestCaseValidatorFactory(IKeyBuilder keyBuilder, IKeyComposerFactory keyComposerFactory, IShaFactory shaFactory)
        {
            _keyBuilder = keyBuilder;
            _keyComposerFactory = keyComposerFactory;
            _shaFactory = shaFactory;
        }

        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup) g))
            {
                foreach (var test in group.Tests.Select(t => (TestCase) t))
                {
                    var testType = group.TestType.ToLower();

                    if (testType == "aft" || testType == "gdt")
                    {
                        list.Add(new TestCaseValidatorAft(test, group, new DeferredTestCaseResolver(_keyBuilder, _keyComposerFactory, _shaFactory)));
                    }
                    else if (testType == "kat")
                    {
                        list.Add(new TestCaseValidatorKat(test));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorNull("Could not determine TestType for TestCase", test.TestCaseId));
                    }
                }
            }

            return list;
        }
    }
}

