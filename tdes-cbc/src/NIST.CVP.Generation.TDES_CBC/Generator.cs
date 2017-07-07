using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.TDES_CBC
{
    public class Generator<TParameters, TTestVectorSet> : GeneratorBase
        where TParameters : IParameters
        where TTestVectorSet : ITestVectorSet
    {
        private readonly ITestVectorFactory<TParameters> _testVectorFactory;
        private readonly IParameterParser<TParameters> _parameterParser;
        private readonly IParameterValidator<TParameters> _parameterValidator;
        private readonly ITestCaseGeneratorFactoryFactory<TTestVectorSet> _testCaseGeneratorFactoryFactory;

        public Generator(ITestVectorFactory<TParameters> testVectorFactory, IParameterParser<TParameters> parameterParser, IParameterValidator<TParameters> parameterValidator, ITestCaseGeneratorFactoryFactory<TTestVectorSet> iTestCaseGeneratorFactoryFactory)
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
