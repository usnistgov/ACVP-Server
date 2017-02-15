using System.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class Generator : GeneratorBase
    {
        private readonly ITestVectorFactory<Parameters> _testVectorFactory;
        private readonly ITestCaseGeneratorFactory _testCaseGeneratorFactory;
        private readonly IParameterParser<Parameters> _parameterParser;
        private readonly IParameterValidator<Parameters> _parameterValidator;
        private readonly IKnownAnswerTestFactory _knownAnswerTestFactory;
        public Generator(ITestVectorFactory<Parameters> testVectorFactory, IParameterParser<Parameters> parameterParser, IParameterValidator<Parameters> parameterValidator, ITestCaseGeneratorFactory testCaseGeneratorFactory, IKnownAnswerTestFactory knownAnswerTestFactory)
        {
            _testVectorFactory = testVectorFactory;
            _testCaseGeneratorFactory = testCaseGeneratorFactory;
            _parameterParser = parameterParser;
            _parameterValidator = parameterValidator;
            _knownAnswerTestFactory = knownAnswerTestFactory;

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
            int testId = 1;
            foreach (var group in testVector.TestGroups.Select(g => (TestGroup)g))
            {
                if (group.NumberOfKeys == 1)
                {
                    //known answer test -- just grab 'em, add a test case Id and move along
                    var kats = _knownAnswerTestFactory.GetKATTestCases(@group.TestType);
                    if (kats.Count == 0)
                    {
                        return new GenerateResponse($"Found 0 {group.Function}: {group.TestType} tests");
                    }
                    foreach (var kat in kats)
                    {
                        kat.TestCaseId = testId++;
                        group.Tests.Add(kat);
                    }
                }
                else
                {
                    var generator = _testCaseGeneratorFactory.GetCaseGenerator(@group, testVector.IsSample);
                    for (int caseNo = 0; caseNo < generator.NumberOfTestCasesToGenerate; ++caseNo)
                    {
                        var testCaseResponse = generator.Generate(@group, testVector.IsSample);
                        if (!testCaseResponse.Success)
                        {
                            return new GenerateResponse(testCaseResponse.ErrorMessage);
                        }
                        var testCase = (TestCase)testCaseResponse.TestCase;
                        testCase.TestCaseId = testId;
                        group.Tests.Add(testCase);
                        testId++;
                    }
                }    
            }   
            return SaveOutputs(requestFilePath, testVector);
        }
    }
}
