using System;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Retrieve <see cref="ITestCaseGenerator{TTestGroup,TTestCase}"/>s based on group information.
    /// </summary>
    /// <typeparam name="TTestGroup">The test group type.</typeparam>
    /// <typeparam name="TTestCase">The test case type</typeparam>
    [Obsolete("Being replaced by ITestCaseGeneratorFactoryAsync")]
    public interface ITestCaseGeneratorFactory<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        /// <summary>
        /// Gets a <see cref="ITestCaseGenerator{TTestGroup,TTestCase}"/> based on the <see cref="testGroup"/> options.
        /// </summary>
        /// <param name="testGroup">The test group to get a test case generator for.</param>
        /// <returns></returns>
        ITestCaseGenerator<TTestGroup, TTestCase> GetCaseGenerator(TTestGroup testGroup);
    }
}