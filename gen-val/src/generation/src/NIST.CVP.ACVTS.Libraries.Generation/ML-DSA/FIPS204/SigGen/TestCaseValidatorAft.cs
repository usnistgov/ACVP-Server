using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;

public class TestCaseValidatorAft : ITestCaseValidatorAsync<TestGroup, TestCase>
{
    private readonly TestCase _expectedResult;
    
    public int TestCaseId => _expectedResult.TestCaseId;

    public TestCaseValidatorAft(TestCase expectedResult)
    {
        _expectedResult = expectedResult;
    }

    public Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
    {
        var errors = new List<string>();
        Dictionary<string, string> expected = new Dictionary<string, string>();
        Dictionary<string, string> provided = new Dictionary<string, string>();
        
        CheckResults(suppliedResult, _expectedResult, errors, expected, provided);

        if (errors.Count > 0)
        {
            return Task.FromResult(new TestCaseValidation
            {
                TestCaseId = suppliedResult.TestCaseId,
                Result = Core.Enums.Disposition.Failed,
                Reason = string.Join(";", errors),
                Expected = showExpected ? expected : null,
                Provided = showExpected ? provided : null
            });
        }

        return Task.FromResult(new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed });
    }
    
    private void CheckResults(TestCase suppliedResult, TestCase expectedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
    {
        if (!suppliedResult.Signature.Equals(expectedResult.Signature))
        {
            errors.Add("Incorrect signature");
            expected.Add(nameof(expectedResult.Signature), expectedResult.Signature.ToHex());
            provided.Add(nameof(suppliedResult.Signature), suppliedResult.Signature.ToHex());
        }
    }
}
