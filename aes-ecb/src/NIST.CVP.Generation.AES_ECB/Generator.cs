using System.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.AES_ECB
{
    public class Generator : GeneratorBase
    {
        private readonly ITestVectorFactory<Parameters> _testVectorFactory;
        private readonly IParameterParser<Parameters> _parameterParser;
        private readonly IParameterValidator<Parameters> _parameterValidator;
        private readonly ITestCaseGeneratorFactoryFactory<TestVectorSet> _testCaseGeneratorFactoryFactory;

        public Generator(ITestVectorFactory<Parameters> testVectorFactory, IParameterParser<Parameters> parameterParser, IParameterValidator<Parameters> parameterValidator, ITestCaseGeneratorFactoryFactory<TestVectorSet> iTestCaseGeneratorFactoryFactory)
        {
            _testVectorFactory = testVectorFactory;
            _parameterParser = parameterParser;
            _parameterValidator = parameterValidator;
            _testCaseGeneratorFactoryFactory = iTestCaseGeneratorFactoryFactory;
        }

        public GenerateResponse Generate(string requestFilePath)
        {
            var parameterResponse = _parameterParser.Parse(requestFilePath);
            if (!parameterResponse.Success)
            {
                return new GenerateResponse(parameterResponse.ErrorMessage);
            }
            var parameters = parameterResponse.ParsedObject;
            var validateResponse = _parameterValidator.Validate((Parameters)parameters);
            if (!validateResponse.Success)
            {
                return new GenerateResponse(validateResponse.ErrorMessage);
            }
            var testVector = _testVectorFactory.BuildTestVectorSet(parameters);
            var testCasesResult = _testCaseGeneratorFactoryFactory.BuildTestCases((TestVectorSet)testVector);
            if (!testCasesResult.Success)
            {
                return testCasesResult;
            }

            return SaveOutputs(requestFilePath, testVector);
        }
    }
}
