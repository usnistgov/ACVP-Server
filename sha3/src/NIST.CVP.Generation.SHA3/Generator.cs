using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.SHA3
{
    public class Generator : GeneratorBase
    {
        private readonly ITestVectorFactory<Parameters> _testVectorFactory;
        private readonly ITestCaseGeneratorFactory _testCaseGeneratorFactory;
        private readonly IParameterParser<Parameters> _parameterParser;
        private readonly IParameterValidator<Parameters> _parameterValidator;

        public Generator(ITestVectorFactory<Parameters> testVectorFactory,
            ITestCaseGeneratorFactory testCaseGeneratorFactory, IParameterParser<Parameters> parameterParser,
            IParameterValidator<Parameters> parameterValidator)
        {
            _testVectorFactory = testVectorFactory;
            _testCaseGeneratorFactory = testCaseGeneratorFactory;
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
            var testCasesResult = _testCaseGeneratorFactory.BuildTestCases(testVector);
            if (!testCasesResult.Success)
            {
                return testCasesResult;
            }

            return SaveOutputs(requestFilePath, testVector);
        }
    }
}
