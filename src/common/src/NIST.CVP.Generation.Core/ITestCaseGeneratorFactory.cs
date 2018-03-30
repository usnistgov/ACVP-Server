namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Retrieve <see cref="ITestCaseGenerator{TTestGroup,TTestCase}"/>s based on group information.
    /// </summary>
    /// <typeparam name="TTestGroup">The test group type.</typeparam>
    /// <typeparam name="TTestCase">The test case type</typeparam>
    public interface ITestCaseGeneratorFactory<in TTestGroup, in TTestCase>
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase
    {
        /// <summary>
        /// Gets a <see cref="ITestCaseGenerator{TTestGroup,TTestCase}"/> based on the <see cref="testGroup"/> options.
        /// </summary>
        /// <param name="testGroup">The test group to get a test case generator for.</param>
        /// <returns></returns>
        ITestCaseGenerator<TTestGroup, TTestCase> GetCaseGenerator(TTestGroup testGroup);
    }
}