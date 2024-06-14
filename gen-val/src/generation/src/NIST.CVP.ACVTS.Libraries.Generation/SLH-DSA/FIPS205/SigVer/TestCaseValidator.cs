using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.SigVer;

public class TestCaseValidator : ITestCaseValidatorAsync<TestGroup, TestCase>
{
    private readonly TestCase _expectedResult;
    public int TestCaseId => _expectedResult.TestCaseId;
    
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
            CheckResults(suppliedResult, _expectedResult, errors, expected, provided);
        
        if (errors.Count > 0)
        {
            return Task.FromResult(new TestCaseValidation
            {
                TestCaseId = TestCaseId,
                Result = Disposition.Failed,
                Reason = string.Join("; ", errors),
                Expected = expected.Count != 0 && showExpected ? expected : null,
                Provided = provided.Count != 0 && showExpected ? provided : null
            });
        }
        
        return Task.FromResult(new TestCaseValidation
        {
            TestCaseId = TestCaseId,
            Result = Disposition.Passed
        });
    }
    
    private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
    {
        if (suppliedResult.TestPassed == null)
            errors.Add($"Could not find {nameof(suppliedResult.TestPassed)}");
    }
    
    private void CheckResults(TestCase suppliedResult, TestCase expectedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
    {
        if (expectedResult.TestPassed != suppliedResult.TestPassed)
        {
            errors.Add(EnumHelpers.GetEnumDescriptionFromEnum(expectedResult.Reason));
            expected.Add(nameof(expectedResult.TestPassed), expectedResult.TestPassed.Value.ToString());
            provided.Add(nameof(suppliedResult.TestPassed), suppliedResult.TestPassed.Value.ToString());
        }
    }
}
