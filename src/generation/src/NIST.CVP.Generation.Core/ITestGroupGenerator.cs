using System.Collections.Generic;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Interface for generating test groups based on a set of <see cref="TParameters"/>
    /// </summary>
    /// <typeparam name="TParameters">The parameters type</typeparam>
    /// <typeparam name="TTestGroup">The test group type</typeparam>
    /// <typeparam name="TTestCase">The test case type</typeparam>
    public interface ITestGroupGenerator<in TParameters, out TTestGroup, TTestCase>
        where TParameters : IParameters
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        /// <summary>
        /// Builds test groups based on an enumeration of parameter properties, 
        /// and test types needed for algorithm assurances.
        /// </summary>
        /// <param name="parameters">The parameters to build groups off of.</param>
        /// <returns></returns>
        IEnumerable<TTestGroup> BuildTestGroups(TParameters parameters);
    }
}