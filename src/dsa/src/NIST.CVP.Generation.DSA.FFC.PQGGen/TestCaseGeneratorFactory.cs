using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IShaFactory _shaFactory;
        private IPQGeneratorValidator _pqGen;
        private IGGeneratorValidator _gGen;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90)
        {
            _random800_90 = random800_90;
            _shaFactory = new ShaFactory();
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            if (testGroup.PQGenMode != PrimeGenMode.None)
            {
                var sha = _shaFactory.GetShaInstance(testGroup.HashAlg);

                switch (testGroup.PQGenMode)
                {
                    case PrimeGenMode.Probable:
                        _pqGen = new ProbablePQGeneratorValidator(sha);
                        break;
                    case PrimeGenMode.Provable:
                        _pqGen = new ProvablePQGeneratorValidator(sha);
                        break;
                }

                return new TestCaseGeneratorPQ(_random800_90, _pqGen);
            }

            if (testGroup.GGenMode != GeneratorGenMode.None)
            {
                switch (testGroup.GGenMode)
                {
                    case GeneratorGenMode.Canonical:
                        var sha = _shaFactory.GetShaInstance(testGroup.HashAlg);
                        _gGen = new CanonicalGeneratorGeneratorValidator(sha);
                        break;
                    case GeneratorGenMode.Unverifiable:
                        _gGen = new UnverifiableGeneratorGeneratorValidator();
                        break;
                }

                return new TestCaseGeneratorG(_random800_90, _gGen);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
