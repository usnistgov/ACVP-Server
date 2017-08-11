using NIST.CVP.Generation.Core;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestCaseGeneratorFactoryFactory : ITestCaseGeneratorFactoryFactory<TestVectorSet>
    {
        private readonly ITestCaseGeneratorFactory<TestGroup, TestCase> _testCaseGeneratorFactory;

        public TestCaseGeneratorFactoryFactory(ITestCaseGeneratorFactory<TestGroup, TestCase> iTestCaseGeneratorFactory)
        {
            _testCaseGeneratorFactory = iTestCaseGeneratorFactory;
        }

        public GenerateResponse BuildTestCases(TestVectorSet testVector)
        {
            var testId = 1;
            foreach(var group in testVector.TestGroups.Select(g => (TestGroup) g))
            {
                var generator = _testCaseGeneratorFactory.GetCaseGenerator(group);

                for(var caseNo = 0; caseNo < generator.NumberOfTestCasesToGenerate; ++caseNo)
                {
                    var testCaseResponse = generator.Generate(group, testVector.IsSample);
                    if (!testCaseResponse.Success)
                    {
                        return new GenerateResponse(testCaseResponse.ErrorMessage);
                    }

                    var testCase = (TestCase)testCaseResponse.TestCase;
                    testCase.TestCaseId = testId;
                    group.Tests.Add(testCase);
                    testId++;
                }

                //LogManager.GetCurrentClassLogger().Log(LogLevel.Debug, $"Group completed {Crypto.RSA.RSAEnumHelpers.SigGenModeToString(group.Mode)}, {group.Modulo}, {Crypto.SHA2.SHAEnumHelpers.HashFunctionToString(group.HashAlg)}.");
            }

            return new GenerateResponse();
        }
    }
}
