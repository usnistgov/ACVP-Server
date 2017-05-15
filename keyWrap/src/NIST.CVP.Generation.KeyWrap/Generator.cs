using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KeyWrap
{
    public class Generator<TParameters, TTestVectorSet> : GeneratorBase
        where TParameters : IParameters
        where TTestVectorSet : ITestVectorSet
    {
        protected readonly ITestVectorFactory<TParameters> _testVectorFactory;
        protected readonly IParameterParser<TParameters> _parameterParser;
        protected readonly IParameterValidator<TParameters> _parameterValidator;
        protected readonly ITestCaseGeneratorFactoryFactory<TTestVectorSet> _testCaseGeneratorFactoryFactory;

        public Generator(ITestVectorFactory<TParameters> testVectorFactory, IParameterParser<TParameters> parameterParser, IParameterValidator<TParameters> parameterValidator, ITestCaseGeneratorFactoryFactory<TTestVectorSet> iTestCaseGeneratorFactoryFactory, IRandom800_90 iRandom800_90)
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
