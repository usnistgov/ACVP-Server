using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public bool IsSample { get; set; }
        [JsonProperty(PropertyName = "direction")]
        public string[] Mode { get; set; }
        /// <summary>
        /// Keying Option 1 - 3 independant key TDES
        /// Keying Option 2 - 2 Key TDES
        /// Keying Option 3 (No longer supported) - 1 Key TDES - only used in KATs
        /// </summary>
        public int[] KeyingOption { get; set; }
    }
}
