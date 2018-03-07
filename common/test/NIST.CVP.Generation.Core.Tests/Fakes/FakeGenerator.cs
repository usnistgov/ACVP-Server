using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeGenerator : Generator<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase>
    {
        public FakeGenerator() : base (null, null, null, null, null) {  }

        public FakeGenerator(
            ITestVectorFactory<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase> testVectorFactory,
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

        // Need an accessor for the protected method inside of GeneratorBase
        public GenerateResponse SaveOutputsTester(string requestFilePath, FakeTestVectorSet testVector)
        {
            return SaveOutputs(requestFilePath, testVector);
        }
    }
}