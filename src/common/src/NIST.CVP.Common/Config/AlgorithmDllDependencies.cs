using System.Collections.Generic;

namespace NIST.CVP.Common.Config
{
    /// <summary>
    /// DLL dependencies for a <see cref="AlgorithmMode"/>
    /// </summary>
    public class AlgorithmDllDependencies
    {
        /// <summary>
        /// The algorithm to be tested
        /// </summary>
        public string Algorithm { get; set; }
        /// <summary>
        /// The algorithm's mode
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// The assembly containing the gen/vals for the algorithm/mode
        /// </summary>
        public string EntryDll { get; set; }
        /// <summary>
        /// Any additional dependencies needed for the <see cref="EntryDll"/> to function.
        /// </summary>
        public HashSet<AdditionalDependencies> AdditionalDependencies { get; set; }
    }
}