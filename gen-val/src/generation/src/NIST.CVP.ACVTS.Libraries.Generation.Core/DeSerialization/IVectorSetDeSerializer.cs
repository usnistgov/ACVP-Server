namespace NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization
{
    /// <summary>
    /// Describes methods for deserializing JSON into a <see cref="TTestVectorSet"/>.
    /// </summary>
    /// <typeparam name="TTestVectorSet">The type that should be hydrated.</typeparam>
    /// <typeparam name="TTestGroup">The test group type</typeparam>
    /// <typeparam name="TTestCase">The test case type</typeparam>
    public interface IVectorSetDeserializer<out TTestVectorSet, TTestGroup, TTestCase>
        where TTestVectorSet : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        /// <summary>
        /// Deserializes provided JSON and returns a <see cref="TTestVectorSet"/>
        /// </summary>
        /// <param name="vectorSetJson">The JSON to use for <see cref="TTestVectorSet"/> hydration.</param>
        /// <returns>The hydrated object</returns>
        TTestVectorSet Deserialize(string vectorSetJson);
    }
}
