namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Performs server to IUT TestCase validation
    /// </summary>
    /// <typeparam name="TTestCase">The test case type to validate</typeparam>
    public interface ITestCaseValidator<in TTestCase>
        where TTestCase : ITestCase
    {
        /// <summary>
        /// Perform validation between the <see cref="suppliedResult"/> and the server.
        /// </summary>
        /// <param name="suppliedResult">The IUT provided results</param>
        /// <returns></returns>
        TestCaseValidation Validate(TTestCase suppliedResult);
        /// <summary>
        /// The unique id of the test case (scoped to the vector set)
        /// </summary>
        int TestCaseId { get; }
    }
}
