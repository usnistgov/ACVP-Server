namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Async
{
    /// <summary>
    /// Retrieve <see cref="ITestCaseGeneratorAsync{TTestGroup,TTestCase}"/>s based on group information.
    /// </summary>
    /// <typeparam name="TTestGroup">The test group type.</typeparam>
    /// <typeparam name="TTestCase">The test case type</typeparam>
    public interface ITestCaseGeneratorFactoryAsync<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        /// <summary>
        /// Gets a <see cref="ITestCaseGeneratorAsync{TTestGroup,TTestCase}"/> based on the <see cref="testGroup"/> options.
        /// </summary>
        /// <param name="testGroup">The test group to get a test case generator for.</param>
        /// <returns></returns>
        ITestCaseGeneratorAsync<TTestGroup, TTestCase> GetCaseGenerator(TTestGroup testGroup);
    }
}
