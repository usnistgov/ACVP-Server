using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        private readonly IShaFactory _shaFactory = new ShaFactory();
        private IPQGeneratorValidator _pqGen;
        private IGGeneratorValidator _gGen;

        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet, IEnumerable<TestCase> suppliedResults)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup)g))
            {
                foreach (var test in group.Tests.Select(t => (TestCase)t))
                {
                    if (group.PQGenMode != PrimeGenMode.None)
                    {
                        var sha = _shaFactory.GetShaInstance(group.HashAlg);

                        switch (group.PQGenMode)
                        {
                            case PrimeGenMode.Probable:
                                _pqGen = new ProbablePQGeneratorValidator(sha);
                                break;
                            case PrimeGenMode.Provable:
                                _pqGen = new ProvablePQGeneratorValidator(sha);
                                break;
                        }

                        list.Add(new TestCaseValidatorPQ(test, _pqGen));
                    }
                    else if (group.GGenMode != GeneratorGenMode.None)
                    {
                        switch (group.GGenMode)
                        {
                            case GeneratorGenMode.Canonical:
                                var sha = _shaFactory.GetShaInstance(group.HashAlg);
                                _gGen = new CanonicalGeneratorGeneratorValidator(sha);
                                break;
                            case GeneratorGenMode.Unverifiable:
                                _gGen = new UnverifiableGeneratorGeneratorValidator();
                                break;
                        }

                        list.Add(new TestCaseValidatorG(test, _gGen));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorNull("Could not find validator for group", test.TestCaseId));
                    }
                }
            }

            return list;
        }
    }
}
