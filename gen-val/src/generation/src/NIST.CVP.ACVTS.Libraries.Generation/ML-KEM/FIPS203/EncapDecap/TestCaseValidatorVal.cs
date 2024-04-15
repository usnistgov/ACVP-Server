using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class TestCaseValidatorVal : ITestCaseValidatorAsync<TestGroup, TestCase>
{
    private readonly TestCase _expectedResult;
    public int TestCaseId => _expectedResult.TestCaseId;

    public TestCaseValidatorVal(TestCase expectedResult)
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
        if (suppliedResult.SharedKey == null)
        {
            errors.Add($"{nameof(suppliedResult.SharedKey)} was not present in the {nameof(TestCase)}");
        }
    }

    private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
    {
        // Need to check if the resulting key is valid or from implicit rejection based on what is expected
        if (_expectedResult.Reason == MLKEMDecapsulationDisposition.None)
        {
            // Reason is good, just check matching shared key
            if (!_expectedResult.SharedKey.Equals(suppliedResult.SharedKey))
            {
                errors.Add($"{nameof(suppliedResult.SharedKey)} does not match expected valid shared key");
                expected.Add(nameof(_expectedResult.SharedKey), _expectedResult.SharedKey.ToHex());
                provided.Add(nameof(suppliedResult.SharedKey), suppliedResult.SharedKey.ToHex());
            }
        }
        else
        {
            // Reason is bad, need to compute expected implicit rejection key
            if (!_expectedResult.SharedKey.Equals(suppliedResult.SharedKey))
            {
                errors.Add($"{nameof(suppliedResult.SharedKey)} does not match expected implicit rejection shared key");
                expected.Add(nameof(_expectedResult.SharedKey), _expectedResult.SharedKey.ToHex());
                provided.Add(nameof(suppliedResult.SharedKey), suppliedResult.SharedKey.ToHex());
            }
        }
    }
}
