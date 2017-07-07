using System.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.SHA2
{
    public class Generator<TParameters, TTestVectorSet> : GeneratorBase
    where TParameters : IParameters
    where TTestVectorSet : ITestVectorSet
    {
        private readonly ITestVectorFactory<TParameters> _testVectorFactory;
        private readonly ITestCaseGeneratorFactoryFactory<TTestVectorSet> _testCaseGeneratorFactoryFactory;
        private readonly IParameterParser<TParameters> _parameterParser;
        private readonly IParameterValidator<TParameters> _parameterValidator;

        public Generator(
            ITestVectorFactory<TParameters> testVectorFactory,
            ITestCaseGeneratorFactoryFactory<TTestVectorSet> testCaseGeneratorFactoryFactory,
            IParameterParser<TParameters> parameterParser,
            IParameterValidator<TParameters> parameterValidator
        )
        {
            _testVectorFactory = testVectorFactory;
            _testCaseGeneratorFactoryFactory = testCaseGeneratorFactoryFactory;
            _parameterParser = parameterParser;
            _parameterValidator = parameterValidator;
        }

        public GenerateResponse Generate(string requestFilePath)
        {
            var parameterResponse = _parameterParser.Parse(requestFilePath);
            if (!parameterResponse.Success)
            {
                return new GenerateResponse(parameterResponse.ErrorMessage);
            }

            var parameters = parameterResponse.ParsedObject;
            var validateResponse = _parameterValidator.Validate(parameters);
            if (!validateResponse.Success)
            {
                return new GenerateResponse(validateResponse.ErrorMessage);
            }

            var testVector = _testVectorFactory.BuildTestVectorSet(parameters);
            var testCasesResult = _testCaseGeneratorFactoryFactory.BuildTestCases((TTestVectorSet)testVector);
            if (!testCasesResult.Success)
            {
                return testCasesResult;
            }

            return SaveOutputs(requestFilePath, testVector);
        }
    }
}
