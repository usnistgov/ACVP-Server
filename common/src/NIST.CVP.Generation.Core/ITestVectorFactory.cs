namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Describes retrieving a <see cref="TTestVectorSet"/>
    /// </summary>
    /// <typeparam name="TParameters">The parameters to use in generating a test vector set</typeparam>
    /// <typeparam name="TTestVectorSet">The test vector set type</typeparam>
    /// <typeparam name="TTestGroup">The test group type</typeparam>
    /// <typeparam name="TTestCase">The test case type</typeparam>
    public interface ITestVectorFactory<in TParameters, out TTestVectorSet, TTestGroup, TTestCase>
        where TParameters : IParameters
        where TTestVectorSet : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        /// <summary>
        /// Returns a <see cref="TTestVectorSet"/> based on <see cref="TParameters"/>
        /// </summary>
        /// <param name="parameters">The parameters to use in creating a <see cref="TTestVectorSet"/></param>
        /// <returns></returns>
        TTestVectorSet BuildTestVectorSet(TParameters parameters);
    }
}
