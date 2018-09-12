using System.Collections.Generic;

namespace NIST.CVP.Common.Config
{
    /// <summary>
    /// Root element of the algorithm dll mapping configuration
    /// </summary>
    public class AlgorithmConfig
    {
        /// <summary>
        /// Set of algorithms and their dll dependencies
        /// </summary>
        public List<AlgorithmDllDependencies> Algorithms { get; set; }
    }
}