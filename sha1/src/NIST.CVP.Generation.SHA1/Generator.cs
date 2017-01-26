using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.SHA1
{
    public class Generator : GeneratorBase
    {
        private readonly ITestVectorFactory<Parameters> _testVectorFactory;
        private readonly IParameterParser<Parameters> _parameterParser;
        private readonly IParameterValidator<Parameters> _parameterValidator;
        private readonly ITestCaseGeneratorFactoryFactory<TestVectorSet> _testCaseGeneratorFactoryFactory;

        public Generator(ITestVectorFactory<Parameters> testVectorFactory, IParameterParser<Parameters> parameterParser, IParameterValidator<Parameters> parameterValidator, ITestCaseGeneratorFactoryFactory<TestVectorSet> testCaseGeneratorFactoryFactory)
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
            var testCasesResult = _testCaseGeneratorFactoryFactory.BuildTestCases((TestVectorSet)testVector);
            if (!testCasesResult.Success)
            {
                return testCasesResult;
            }

            return SaveOutputs(requestFilePath, testVector);
        }
    }
}
