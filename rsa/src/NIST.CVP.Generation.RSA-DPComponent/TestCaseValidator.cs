using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_DPComponent
{
    public class TestCaseValidator : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, ManyEncryptionResult> _deferredTestCaseResolver;
        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidator(TestCase expectedResult, IDeferredTestCaseResolver<TestGroup, TestCase, ManyEncryptionResult> resolver)
        {
            _expectedResult = expectedResult;
            _deferredTestCaseResolver = resolver;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();

            if (_expectedResult.ResultsArray.Count != suppliedResult.ResultsArray.Count)
            {
                errors.Add("Improper number of replies received");
            }
            else if (suppliedResult.ResultsArray.Count(ra => ra.FailureTest) < _expectedResult.ResultsArray.Count / 3)
            {
                errors.Add("Not enough failures detected");
            }
            else
            {
                var computedResults = _deferredTestCaseResolver.CompleteDeferredCrypto(null, _expectedResult, suppliedResult);

                for (var i = 0; i < computedResults.AlgoArrayResponses.Count; i++)
                {
                    var computedResult = computedResults.AlgoArrayResponses[i];
                    var cipherText = _expectedResult.ResultsArray[i].CipherText.ToPositiveBigInteger();
                    var n = suppliedResult.ResultsArray[i].Key.PubKey.N;

                    // Should the test case pass
                    var expectedPass = (cipherText < n - 1);

                    if (computedResult.FailureTest)
                    {
                        // IUT says the test case should fail, verify that
                        if (expectedPass)
                        {
                            // Should not have failed
                            errors.Add($"Test should not have failed, cipherText < n - 1 on iteration {i}");
                        }
                        else
                        {
                            // IUT and Server agree the test case should fail
                        }
                    }
                    else
                    {
                        // IUT says the test case should pass (generate a plaintext), verify that, then verify correctness
                        if (expectedPass)
                        {
                            if (suppliedResult.ResultsArray[i].PlainText == null)
                            {
                                errors.Add($"Could not find plaintext for test on iteration {i}");
                            }
                            else
                            {
                                var iutPlainText = suppliedResult.ResultsArray[i].PlainText.ToPositiveBigInteger();
                                var computedPlainText = computedResult.PlainText.ToPositiveBigInteger();

                                if (iutPlainText == computedPlainText)
                                {
                                    // IUT and server agree on the plaintext
                                }
                                else
                                {
                                    errors.Add($"Incorrect plaintext computed for test on iteration {i}");
                                }
                            }
                        }
                    }
                }
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Failed, Reason = string.Join(";", errors) };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }
    }
}

