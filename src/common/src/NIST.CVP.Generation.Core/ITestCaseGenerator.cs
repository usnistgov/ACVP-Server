namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Generates test cases.  Performs crypto operations unless deferred or a known answer test.
    /// </summary>
    /// <typeparam name="TTestGroup">The test group information.</typeparam>
    /// <typeparam name="TTestCase">The test case type.</typeparam>
    public interface ITestCaseGenerator<in TTestGroup, in TTestCase>
        where TTestGroup : ITestGroup 
        where TTestCase : ITestCase
    {
        /// <summary>
        /// The number of test cases to generate for this test type and group.
        /// </summary>
        int NumberOfTestCasesToGenerate { get; }
        /// <summary>
        /// Generates a <see cref="TTestCase"/>.
        /// </summary>
        /// <param name="group">The test group information</param>
        /// <param name="isSample">Is this a sample test?</param>
        /// <returns></returns>
        TestCaseGenerateResponse Generate(TTestGroup @group, bool isSample);
        /// <summary>
        /// Generates a <see cref="TTestCase"/>.
        /// TODO is this needed?  Could it not be accomplished with the other method?
        /// </summary>
        /// <param name="group">The test group information</param>
        /// <param name="testCase">The testCase to hydrate</param>
        /// <returns></returns>
        TestCaseGenerateResponse Generate(TTestGroup @group, TTestCase testCase);
    }
}