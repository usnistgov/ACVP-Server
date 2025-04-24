using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.AEAD128;
public class TestCaseValidatorEncrypt : ITestCaseValidatorAsync<TestGroup, TestCase>
{
    public int TestCaseId => _expectedResult.TestCaseId;

    private readonly TestCase _expectedResult;

    public TestCaseValidatorEncrypt(TestCase expectedResult)
    {
        _expectedResult = expectedResult;
    }

    public Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
    {
        var errors = new List<string>();
        var expected = new Dictionary<string, string>();
        var provided = new Dictionary<string, string>();

        ValidateResultPresent(suppliedResult, errors);
        if (errors.Count == 0)
        {
            CheckResults(suppliedResult, errors, expected, provided);
        }

        if (errors.Count > 0)
        {
            return Task.FromResult(new TestCaseValidation
            {
                TestCaseId = suppliedResult.TestCaseId,
                Result = Core.Enums.Disposition.Failed,
                Reason = string.Join("; ", errors),
                Expected = expected.Count != 0 && showExpected ? expected : null,
                Provided = provided.Count != 0 && showExpected ? provided : null
            });
        }

        return Task.FromResult(new TestCaseValidation
        {
            TestCaseId = TestCaseId,
            Result = Core.Enums.Disposition.Passed
        });
    }

    private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
    {
        if (suppliedResult.Tag == null)
        {
            errors.Add($"{nameof(suppliedResult.Tag)} was not present in the {nameof(TestCase)}");
        }
        if (suppliedResult.Ciphertext == null)
        {
            errors.Add($"{nameof(suppliedResult.Ciphertext)} was not present in the {nameof(TestCase)}");
        }
    }

    private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
    {
        if (!_expectedResult.Tag.Equals(suppliedResult.Tag))
        {
            errors.Add($"{nameof(suppliedResult.Tag)} does not match");
            expected.Add(nameof(_expectedResult.Tag), _expectedResult.Tag.ToHex());
            provided.Add(nameof(suppliedResult.Tag), suppliedResult.Tag.ToHex());
        }
        if (!_expectedResult.Ciphertext.Equals(suppliedResult.Ciphertext))
        {
            errors.Add($"{nameof(suppliedResult.Ciphertext)} does not match");
            expected.Add(nameof(_expectedResult.Ciphertext), _expectedResult.Ciphertext.ToHex());
            provided.Add(nameof(suppliedResult.Ciphertext), suppliedResult.Ciphertext.ToHex());
        }
    }
}
