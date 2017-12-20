using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CBCI
{
    public class TestCaseGeneratorFactoryFactory : ITestCaseGeneratorFactoryFactory<TestVectorSet>
    {
        private readonly IKnownAnswerTestFactory _iKnownAnswerTestFactory;
        private readonly ITestCaseGeneratorFactory<TestGroup, TestCase> _iTestCaseGeneratorFactory;

        public TestCaseGeneratorFactoryFactory(IKnownAnswerTestFactory iKnownAnswerTestFactory, ITestCaseGeneratorFactory<TestGroup, TestCase> iTestCaseGeneratorFactory)
        {
            _iKnownAnswerTestFactory = iKnownAnswerTestFactory;
            _iTestCaseGeneratorFactory = iTestCaseGeneratorFactory;
        }

        public GenerateResponse BuildTestCases(TestVectorSet testVectorSet)
        {
            int testId = 1;
            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup)g))
            {
                if (group.TestType.ToLower() != "multiblockmessage" && group.TestType.ToLower() != "mct")
                {
                    //known answer test -- just grab 'em, add a test case Id and move along
                    var kats = _iKnownAnswerTestFactory.GetKATTestCases(@group.TestType, @group.Function);
                    //Decrypt kats already have TestCaseId set. Decrypt and Encrypt are not separate
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
                    var generator = _iTestCaseGeneratorFactory.GetCaseGenerator(@group);
                    for (int caseNo = 0; caseNo < generator.NumberOfTestCasesToGenerate; ++caseNo)
                    {
                        var testCaseResponse = generator.Generate(@group, testVectorSet.IsSample);
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

            return new GenerateResponse();
        }
    }
}
