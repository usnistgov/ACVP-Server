using System.Collections.Generic;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Interface for generating test groups based on a set of <see cref="TParameters"/>
    /// </summary>
    /// <typeparam name="TParameters">The parameters type</typeparam>
    public interface ITestGroupGenerator<in TParameters>
        where TParameters : IParameters
    {
        /// <summary>
        /// Builds test groups based on an enumeration of parameter properties, 
        /// and test types needed for algorithm assurances.
        /// </summary>
        /// <param name="parameters">The parameters to build groups off of.</param>
        /// <returns></returns>
        IEnumerable<ITestGroup> BuildTestGroups(TParameters parameters);
    }
}