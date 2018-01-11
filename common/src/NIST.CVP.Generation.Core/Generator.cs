using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.Core
{
    public class Generator<TParameters, TTestVectorSet, TTestGroup, TTestCase> : GeneratorBase, IGenerator
        where TParameters : IParameters
        where TTestVectorSet : ITestVectorSet
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase
    {
        protected readonly ITestVectorFactory<TParameters> _testVectorFactory;
        protected readonly IParameterParser<TParameters> _parameterParser;
        protected readonly IParameterValidator<TParameters> _parameterValidator;
        protected readonly ITestCaseGeneratorFactoryFactory<TTestVectorSet, TTestGroup, TTestCase> _testCaseGeneratorFactoryFactory;

        public Generator(
            ITestVectorFactory<TParameters> testVectorFactory, 
            IParameterParser<TParameters> parameterParser, 
            IParameterValidator<TParameters> parameterValidator, 
            ITestCaseGeneratorFactoryFactory<TTestVectorSet, TTestGroup, TTestCase> iTestCaseGeneratorFactoryFactory
        )
        {
            _testVectorFactory = testVectorFactory;
            _parameterParser = parameterParser;
            _parameterValidator = parameterValidator;
            _testCaseGeneratorFactoryFactory = iTestCaseGeneratorFactoryFactory;
        }

        public virtual GenerateResponse Generate(string requestFilePath)
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
