using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.Core.DeSerialization
{
    /// <summary>
    /// Describes methods for serializing a <see cref="ITestVectorSet{TTestGroup, TTestCase}"/>
    /// </summary>
    /// <typeparam name="TTestVectorSet">THe vector set type</typeparam>
    /// /// <typeparam name="TTestGroup">THe group type</typeparam>
    /// /// <typeparam name="TTestCase">THe test case type</typeparam>
    public interface IVectorSetSerializer<in TTestVectorSet, TTestGroup, TTestCase>
        where TTestVectorSet : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        /// <summary>
        /// Serializes a <see cref="ITestVectorSet{TTestGroup, TTestCase}"/> 
        /// to the specified <see cref="Projection"/> as a returned JSON string.
        /// </summary>
        /// <param name="vectorSet">The vector set to serialize</param>
        /// <param name="projection">The project/intended audience to serialize the VectorSet for.</param>
        /// <returns>JSON string</returns>
        string Serialize(TTestVectorSet vectorSet, Projection projection);
    }
}