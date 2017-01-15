namespace NIST.CVP.Generation.Core
{
    public interface ITestCaseGenerator<in TTestGroup, in TTestCase>
        where TTestGroup : ITestGroup 
        where TTestCase : ITestCase
    {
        int NumberOfTestCasesToGenerate { get; }
        TestCaseGenerateResponse Generate(TTestGroup @group, bool isSample);
        TestCaseGenerateResponse Generate(TTestGroup @group, TTestCase testCase);
    }
}