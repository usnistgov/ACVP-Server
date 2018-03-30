namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Used to create <see cref="TTestCase"/>s for a <see cref="TTestVectorSet"/>.
    /// </summary>
    /// <typeparam name="TTestVectorSet">The vector set type.</typeparam>
    /// <typeparam name="TTestGroup">The test group type.</typeparam>
    /// <typeparam name="TTestCase">The test case type.</typeparam>
    public interface ITestCaseGeneratorFactoryFactory<in TTestVectorSet, TTestGroup, TTestCase>
        where TTestVectorSet : ITestVectorSet
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase
    {
        /// <summary>
        /// Create <see cref="TTestCase"/> based on <see cref="testVectorSet"/>
        /// </summary>
        /// <param name="testVectorSet">The <see cref="TTestVectorSet"/> to base test case creation off of.</param>
        /// <returns></returns>
        GenerateResponse BuildTestCases(TTestVectorSet testVectorSet);
    }
}
