using System.Collections.Generic;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Represents a group of tests, generally separated by registration enumerations and/or specific
    /// algorithm test scenarios.
    /// </summary>
    public interface ITestGroup
    {
        int TestGroupId { get; set; }
        /// <summary>
        /// The test type (AFT, KAT, VOT, etc)
        /// </summary>
        string TestType { get; }
        /// <summary>
        /// The <see cref="ITestCase"/>s belonging to the group
        /// </summary>
        List<ITestCase> Tests { get; }
    }
}
