using System.Collections.Generic;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Represents a group of tests, generally separated by registration enumerations and/or specific
    /// algorithm test scenarios.
    /// </summary>
    public interface ITestGroup
    {
        /// <summary>
        /// The test type (AFT, KAT, VOT, etc)
        /// </summary>
        string TestType { get; }
        /// <summary>
        /// The <see cref="ITestCase"/>s belonging to the group
        /// </summary>
        List<ITestCase> Tests { get; }
        /// <summary>
        /// Attempts to merge together the server's <see cref="Tests"/> with the IUTs <see cref="testsToMerge"/>
        /// </summary>
        /// <param name="testsToMerge">Tests to consolidate against <see cref="Tests"/></param>
        /// <returns></returns>
        bool MergeTests(List<ITestCase> testsToMerge);
    }
}
