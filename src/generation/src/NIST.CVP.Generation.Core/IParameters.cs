using Newtonsoft.Json;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Describes registration parameters
    /// </summary>
    public interface IParameters
    {
        [JsonProperty(PropertyName = "vsId")]
        int VectorSetId { get; }
        /// <summary>
        /// The algorithm to test
        /// </summary>
        string Algorithm { get; }
        /// <summary>
        /// The mode the algorithm is running in (can be null/empty)
        /// </summary>
        string Mode { get; }
        /// <summary>
        /// The testing revision for the algorithm.
        /// </summary>
        string Revision { get; }
        /// <summary>
        /// Is the algorithm running in "sample" mode?  
        /// (Can impact number of tests generated and how they are generated)
        /// </summary>
        bool IsSample { get; }
        /// <summary>
        /// Is this implementation designed for further conformances with other specifications
        /// </summary>
        string[] Conformances { get; }
    }
}
