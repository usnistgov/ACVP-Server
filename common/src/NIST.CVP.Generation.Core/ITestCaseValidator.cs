namespace NIST.CVP.Generation.Core
{
    public interface ITestCaseValidator<in TTestCase>
        where TTestCase : ITestCase
    {
        TestCaseValidation Validate(TTestCase suppliedResult);
        int TestCaseId { get; }
    }
}
