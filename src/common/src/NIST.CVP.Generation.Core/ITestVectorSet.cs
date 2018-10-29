using System.Collections.Generic;
using Newtonsoft.Json;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// A collection of tests for a crypto algorithm.
    /// </summary>
    public interface ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        [JsonProperty(PropertyName = "vsId")]
        int VectorSetId { get; set; }
        /// <summary>
        /// The algorithm being tested
        /// </summary>
        [JsonProperty(PropertyName = "algorithm")]
        string Algorithm { get; set; }
        /// <summary>
        /// The algorithm mode.
        /// </summary>
        [JsonProperty(PropertyName = "mode")]
        string Mode { get; set; }
        /// <summary>
        /// Is this a sample vector set?
        /// </summary>
        [JsonProperty(PropertyName = "isSample")]
        bool IsSample { get; set; }
        /// <summary>
        /// The test groups associated with the vector set
        /// </summary>
        [JsonProperty(PropertyName = "testGroups")]
        List<TTestGroup> TestGroups { get; set; }
    }
}
