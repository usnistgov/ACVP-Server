using System.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.SHA2
{
    public class Generator : GeneratorBase
    {
        private readonly ITestVectorFactory<Parameters> _testVectorFactory;
        private readonly ITestCaseGeneratorFactory _testCaseGeneratorFactory;
        private readonly IParameterParser<Parameters> _parameterParser;
        private readonly IParameterValidator<Parameters> _parameterValidator;

        public Generator(ITestVectorFactory<Parameters> testVectorFactory, IParameterParser<Parameters> parameterParser,
            IParameterValidator<Parameters> paramterValidator, ITestCaseGeneratorFactory testCaseGeneratorFactory)
        {
            _testVectorFactory = testVectorFactory;
            _testCaseGeneratorFactory = testCaseGeneratorFactory;
            _parameterParser = parameterParser;
            _parameterValidator = paramterValidator;
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
            var testCasesResult = _testCaseGeneratorFactory.BuildTestCases(testVector);
            if (!testCasesResult.Success)
            {
                return testCasesResult;
            }

            return SaveOutputs(requestFilePath, testVector);
        }
    }
}
