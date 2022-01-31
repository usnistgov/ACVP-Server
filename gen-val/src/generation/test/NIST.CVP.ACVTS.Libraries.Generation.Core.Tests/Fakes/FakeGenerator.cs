using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Parsers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Tests.Fakes
{
    public class FakeGenerator : Generator<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase>
    {
        public FakeGenerator() : base(null, null, null, null, null, null) { }

        public FakeGenerator(
            IOracle oracle,
            ITestVectorFactoryAsync<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase> testVectorFactory,
            IParameterParser<FakeParameters> parameterParser,
            IParameterValidator<FakeParameters> parameterValidator,
            ITestCaseGeneratorFactoryFactory<FakeTestVectorSet, FakeTestGroup, FakeTestCase> iTestCaseGeneratorFactoryFactory,
            IVectorSetSerializer<FakeTestVectorSet, FakeTestGroup, FakeTestCase> vectorSetSerializer
        ) : base(
            oracle,
            testVectorFactory,
            parameterParser,
            parameterValidator,
            iTestCaseGeneratorFactoryFactory,
            vectorSetSerializer
        )
        { }
    }
}
