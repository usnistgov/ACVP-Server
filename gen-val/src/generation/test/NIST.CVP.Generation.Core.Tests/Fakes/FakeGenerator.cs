using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeGenerator : Generator<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase>
    {
        public FakeGenerator() : base (null, null, null, null, null) {  }

        public FakeGenerator(
            ITestVectorFactoryAsync<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase> testVectorFactory,
            IParameterParser<FakeParameters> parameterParser,
            IParameterValidator<FakeParameters> parameterValidator,
            ITestCaseGeneratorFactoryFactory<FakeTestVectorSet, FakeTestGroup, FakeTestCase> iTestCaseGeneratorFactoryFactory,
            IVectorSetSerializer<FakeTestVectorSet, FakeTestGroup, FakeTestCase> vectorSetSerializer
        ) : base (
            testVectorFactory,
            parameterParser,
            parameterValidator,
            iTestCaseGeneratorFactoryFactory,
            vectorSetSerializer
        ) { }
    }
}