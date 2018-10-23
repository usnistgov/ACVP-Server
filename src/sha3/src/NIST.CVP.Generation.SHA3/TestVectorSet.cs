using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA3
{
    public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
    {
        private string _algorithm = string.Empty; 

        /// <summary>
        /// Making some assumptions here (at least currently) that only a single digest size is being provided.
        /// The intention is that the IUT will be providing the full algorithm and digest size as their "algorithm";
        /// though the genvals can take in multiple digest sizes.
        ///
        /// Using a getter to provide the algorithm based on group information
        /// </summary>

        public string Algorithm
        {
            get
            {
                var firstTg = TestGroups?[0];

                if (firstTg == null)
                {
                    return _algorithm;
                }
                
                return $"{firstTg.Function}-{firstTg.DigestSize}";
            }
            set => _algorithm = value;
        }
        
        [JsonIgnore]
        public string Mode { get; set; } = string.Empty;
        public bool IsSample { get; set; }
        public List<TestGroup> TestGroups { get; set; } = new List<TestGroup>();
    }
}
