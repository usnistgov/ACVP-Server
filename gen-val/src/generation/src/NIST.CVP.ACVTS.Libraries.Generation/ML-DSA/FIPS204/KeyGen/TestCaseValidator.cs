using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.KeyGen;

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
        if (suppliedResult.PublicKey == null)
        {
            errors.Add($"{nameof(suppliedResult.PublicKey)} was not present in the {nameof(TestCase)}");
        }

        if (suppliedResult.PrivateKey == null)
        {
            errors.Add($"{nameof(suppliedResult.PrivateKey)} was not present in the {nameof(TestCase)}");
        }
    }

    private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
    {
        if (!_expectedResult.PublicKey.Equals(suppliedResult.PublicKey))
        {
            errors.Add($"{nameof(suppliedResult.PublicKey)} does not match");
            expected.Add(nameof(_expectedResult.PublicKey), _expectedResult.PublicKey.ToHex());
            provided.Add(nameof(suppliedResult.PublicKey), suppliedResult.PublicKey.ToHex());
        }
        
        if (!_expectedResult.PrivateKey.Equals(suppliedResult.PrivateKey))
        {
            errors.Add($"{nameof(suppliedResult.PrivateKey)} does not match");
            expected.Add(nameof(_expectedResult.PrivateKey), _expectedResult.PrivateKey.ToHex());
            provided.Add(nameof(suppliedResult.PrivateKey), suppliedResult.PrivateKey.ToHex());
        }
    }
}
