using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Generation.AES_ECB.Parsers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Resolvers;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_ECB
{
    public class Generator : GeneratorBase
    {
        private readonly ITestVectorFactory _testVectorFactory;
        private readonly ITestCaseGeneratorFactory _testCaseGeneratorFactory;
        private readonly IParameterParser _parameterParser;
        private readonly IParameterValidator _parameterValidator;

        public Generator(ITestVectorFactory testVectorFactory, IParameterParser parameterParser, IParameterValidator parameterValidator, ITestCaseGeneratorFactory testCaseGeneratorFactory)
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
            int testId = 1;
            foreach (var group in testVector.TestGroups.Select(g => (TestGroup)g))
            {
                var generator = _testCaseGeneratorFactory.GetCaseGenerator(group.Function);
                for (int caseNo = 0; caseNo < NUMBER_OF_CASES; ++caseNo)
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
            return SaveOutputs(requestFilePath, testVector);
        }
    }
}
