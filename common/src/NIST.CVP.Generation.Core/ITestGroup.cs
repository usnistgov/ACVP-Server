using System.Collections.Generic;
using Newtonsoft.Json;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Represents a group of tests, generally separated by registration enumerations and/or specific
    /// algorithm test scenarios.
    /// </summary>
    public interface ITestGroup<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        /// <summary>
        /// The test group's ID
        /// </summary>
        [JsonProperty(PropertyName = "tgId")]
        int TestGroupId { get; set; }
        /// <summary>
        /// The test type (AFT, KAT, VOT, etc)
        /// </summary>
        [JsonProperty(PropertyName = "testType")]
        string TestType { get; }
        /// <summary>
        /// The <see cref="TTestCase"/>s belonging to the group
        /// </summary>
        [JsonProperty(PropertyName = "tests")]
        List<TTestCase> Tests { get; set; }
    }
}
