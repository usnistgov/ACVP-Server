namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Performs server to IUT TestCase validation
    /// </summary>
    /// <typeparam name="TTestGroup">The test group type</typeparam>
    /// <typeparam name="TTestCase">The test case type to validate</typeparam>
    public interface ITestCaseValidator<TTestGroup, in TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        /// <summary>
        /// Perform validation between the <see cref="suppliedResult"/> and the server.
        /// </summary>
        /// <param name="suppliedResult">The IUT provided results</param>
        /// <param name="showExpected">Shows the expected value for incorrect responses</param>
        /// <returns></returns>
        TestCaseValidation Validate(TTestCase suppliedResult, bool showExpected);

        /// <summary>
        /// The unique id of the test case (scoped to the vector set)
        /// </summary>
        int TestCaseId { get; }
    }
}
