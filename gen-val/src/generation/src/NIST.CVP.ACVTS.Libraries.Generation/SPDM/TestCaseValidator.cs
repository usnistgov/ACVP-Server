using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SPDM;
public class TestCaseValidator : ITestCaseValidatorAsync<TestGroup, TestCase>
{
    public int TestCaseId => _expectedResult.TestCaseId;

    private readonly TestCase _expectedResult;

    public TestCaseValidator(TestCase expectedResult)
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
        if (suppliedResult.RequestHandshakeSecret == null)
        {
            errors.Add($"{nameof(suppliedResult.RequestHandshakeSecret)} was not present in the {nameof(TestCase)}");
        }
        if (suppliedResult.ResponseHandshakeSecret == null)
        {
            errors.Add($"{nameof(suppliedResult.ResponseHandshakeSecret)} was not present in the {nameof(TestCase)}");
        }
        if (suppliedResult.RequestDataSecret == null)
        {
            errors.Add($"{nameof(suppliedResult.RequestDataSecret)} was not present in the {nameof(TestCase)}");
        }
        if (suppliedResult.ResponseDataSecret == null)
        {
            errors.Add($"{nameof(suppliedResult.ResponseDataSecret)} was not present in the {nameof(TestCase)}");
        }
        if (suppliedResult.ExportMasterSecret == null)
        {
            errors.Add($"{nameof(suppliedResult.ExportMasterSecret)} was not present in the {nameof(TestCase)}");
        }
    }

    private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
    {
        if (!_expectedResult.RequestHandshakeSecret.Equals(suppliedResult.RequestHandshakeSecret))
        {
            errors.Add($"{nameof(suppliedResult.RequestHandshakeSecret)} does not match");
            expected.Add(nameof(_expectedResult.RequestHandshakeSecret), _expectedResult.RequestHandshakeSecret.ToHex());
            provided.Add(nameof(suppliedResult.RequestHandshakeSecret), suppliedResult.RequestHandshakeSecret.ToHex());
        }
        if (!_expectedResult.ResponseHandshakeSecret.Equals(suppliedResult.ResponseHandshakeSecret))
        {
            errors.Add($"{nameof(suppliedResult.ResponseHandshakeSecret)} does not match");
            expected.Add(nameof(_expectedResult.ResponseHandshakeSecret), _expectedResult.ResponseHandshakeSecret.ToHex());
            provided.Add(nameof(suppliedResult.ResponseHandshakeSecret), suppliedResult.ResponseHandshakeSecret.ToHex());
        }
        if (!_expectedResult.RequestDataSecret.Equals(suppliedResult.RequestDataSecret))
        {
            errors.Add($"{nameof(suppliedResult.RequestDataSecret)} does not match");
            expected.Add(nameof(_expectedResult.RequestDataSecret), _expectedResult.RequestDataSecret.ToHex());
            provided.Add(nameof(suppliedResult.RequestDataSecret), suppliedResult.RequestDataSecret.ToHex());
        }
        if (!_expectedResult.ResponseDataSecret.Equals(suppliedResult.ResponseDataSecret))
        {
            errors.Add($"{nameof(suppliedResult.ResponseDataSecret)} does not match");
            expected.Add(nameof(_expectedResult.ResponseDataSecret), _expectedResult.ResponseDataSecret.ToHex());
            provided.Add(nameof(suppliedResult.ResponseDataSecret), suppliedResult.ResponseDataSecret.ToHex());
        }
        if (!_expectedResult.ExportMasterSecret.Equals(suppliedResult.ExportMasterSecret))
        {
            errors.Add($"{nameof(suppliedResult.ExportMasterSecret)} does not match");
            expected.Add(nameof(_expectedResult.ExportMasterSecret), _expectedResult.ExportMasterSecret.ToHex());
            provided.Add(nameof(suppliedResult.ExportMasterSecret), suppliedResult.ExportMasterSecret.ToHex());
        }
    }
}
