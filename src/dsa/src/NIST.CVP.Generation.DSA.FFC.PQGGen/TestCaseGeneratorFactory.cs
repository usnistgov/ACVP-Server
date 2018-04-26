using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IPQGeneratorValidatorFactory _pqGeneratorFactory;
        private readonly IGGeneratorValidatorFactory _gGeneratorFactory;
        private readonly IShaFactory _shaFactory;
        private readonly IRandom800_90 _random800_90;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IShaFactory shaFactory, IPQGeneratorValidatorFactory pqGeneratorFactory, IGGeneratorValidatorFactory gGeneratorFactory)
        {
            _random800_90 = random800_90;
            _shaFactory = shaFactory;
            _pqGeneratorFactory = pqGeneratorFactory;
            _gGeneratorFactory = gGeneratorFactory;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            if (testGroup.PQGenMode != PrimeGenMode.None)
            {
                return new TestCaseGeneratorPQ(_random800_90, _shaFactory, _pqGeneratorFactory);
            }

            if (testGroup.GGenMode != GeneratorGenMode.None)
            {
                return new TestCaseGeneratorG(_random800_90, _shaFactory, _pqGeneratorFactory, _gGeneratorFactory);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
