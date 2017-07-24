using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        private readonly ITestCaseGeneratorFactory<TestGroup, TestCase> _testCaseGeneratorFactory;

        public TestCaseValidatorFactory(ITestCaseGeneratorFactory<TestGroup, TestCase> testCaseGeneratorFactory)
        {
            _testCaseGeneratorFactory = testCaseGeneratorFactory;
        }

        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet,
            IEnumerable<TestCase> suppliedResults)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup) g))
            {
                var generator = _testCaseGeneratorFactory.GetCaseGenerator(group);
                foreach (var test in group.Tests.Select(t => (TestCase) t))
                {
                    // Build this to be the expectedResult, but we might need information from the client's TestCase first
                    var workingTest = test;

                    // Fill in the rest of the information for the test. Only AFT tests can be deferred so do a sanity check
                    if (test.Deferred && group.TestType.ToLower() == "aft")
                    {
                        // This is kinda gross but gets the job done without creating a ITestCaseDeferredGeneratorFactory and adding
                        // the GDT/KAT generators to the deferred interface when they aren't deferred
                        var deferredGenerator = generator as IDeferredTestCaseGenerator<TestGroup, TestCase>;
                        if (deferredGenerator == null)
                        {
                            list.Add(new TestCaseValidatorNull($"Case should not be deferred for TestCase = {test.TestCaseId}", test.TestCaseId));
                            continue;
                        }

                        var matchingResult = suppliedResults.FirstOrDefault(r => r.TestCaseId == test.TestCaseId);
                        var combinedTestCaseResponse = deferredGenerator.RecombineTestCases(group, matchingResult, test);
                        if (!combinedTestCaseResponse.Success)
                        {
                            list.Add(new TestCaseValidatorNull($"Could not recombine TestCase = {matchingResult.TestCaseId}", matchingResult.TestCaseId));
                            continue;
                        }

                        var combinedTestCase = (TestCase) combinedTestCaseResponse.TestCase;

                        var genResult = deferredGenerator.CompleteDeferredTestCase(group, combinedTestCase);
                        if (!genResult.Success)
                        {
                            list.Add(new TestCaseValidatorNull($"Could not generate results for TestCase = {matchingResult.TestCaseId}", matchingResult.TestCaseId));
                            continue;
                        }

                        workingTest = (TestCase) genResult.TestCase;
                    }

                    if (group.TestType.ToLower() == "aft")
                    {
                        list.Add(new TestCaseValidatorAFT(workingTest));
                    }
                    else if (group.TestType.ToLower() == "gdt")
                    {
                        list.Add(new TestCaseValidatorGDT(workingTest, group));
                    }
                    else if (group.TestType.ToLower() == "kat")
                    {
                        list.Add(new TestCaseValidatorKAT(workingTest));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorNull($"Could not determine TestType for TestCase", workingTest.TestCaseId));
                    }
                }
            }

            return list;
        }
    }
}
