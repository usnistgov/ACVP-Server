using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_DPComponent
{
    public class TestCaseValidator : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly TestGroup _group;
        private readonly TestCase _expectedResult;
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, ManyEncryptionResult> _deferredTestCaseResolver;
        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidator(TestGroup group, TestCase expectedResult, IDeferredTestCaseResolver<TestGroup, TestCase, ManyEncryptionResult> resolver)
        {
            _group = group;
            _expectedResult = expectedResult;
            _deferredTestCaseResolver = resolver;
        }

        public TestCaseValidation Validate(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            if (_expectedResult.ResultsArray.Count != suppliedResult.ResultsArray.Count)
            {
                errors.Add("Improper number of replies received");
            }
            else if (suppliedResult.ResultsArray.Count(ra => ra.FailureTest) != _group.TotalFailingCases)
            {
                errors.Add("Incorrect number of failures detected");
            }
            else
            {
                var computedResults = _deferredTestCaseResolver.CompleteDeferredCrypto(_group, _expectedResult, suppliedResult);

                for (var i = 0; i < computedResults.AlgoArrayResponses.Count; i++)
                {
                    var computedResult = computedResults.AlgoArrayResponses[i];
                    var iutResult = suppliedResult.ResultsArray[i];
                    var serverPrompt = _expectedResult.ResultsArray[i];

                    if (computedResult.FailureTest)
                    {
                        // IUT Failure Test must equal computed Failure Test if it's a failure case
                        if (iutResult.FailureTest)
                        {
                            // Good, Pass
                        }
                        else
                        {
                            errors.Add($"Test case should have failed, 1 < cipherText < n - 1 not satisfied on iteration {i}");
                        }
                    }
                    else
                    {
                        // Prompt CT, must equal computed CT if it's not a failure case
                        if (serverPrompt.CipherText.Equals(computedResult.CipherText))
                        {
                            // Good, pass
                        }
                        else
                        {
                            errors.Add($"Computed cipherText from encryption does not match IUT value on iteration {i}");
                            expected.Add($"{nameof(serverPrompt.CipherText)} {i}", serverPrompt.CipherText.ToHex());
                            provided.Add($"{nameof(computedResult.CipherText)} {i}", computedResult.CipherText.ToHex());
                        }
                    }
                }
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation 
                { 
                    TestCaseId = suppliedResult.TestCaseId, 
                    Result = Core.Enums.Disposition.Failed, 
                    Reason = string.Join(";", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }
    }
}

