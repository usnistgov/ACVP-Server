using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KMAC
{
    public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
    {
        public int VectorSetId { get; set; }
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

                var algo = "KMAC";
                var digestSize = firstTg.DigestSize;

                return $"{algo}-{digestSize}";
            }
            set => _algorithm = value;
        }

        [JsonIgnore]
        public string Mode { get; set; }
        public bool IsSample { get; set; }

        public List<TestGroup> TestGroups { get; set; } = new List<TestGroup>();
    }
}
