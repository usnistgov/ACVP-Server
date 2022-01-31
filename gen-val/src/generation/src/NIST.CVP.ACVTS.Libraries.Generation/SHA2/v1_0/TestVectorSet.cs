using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA2.v1_0
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
                return firstTg == null ? _algorithm : ShaAttributes.GetShaAttributes(firstTg.Function, firstTg.DigestSize).name;
            }

            set => _algorithm = value;
        }

        public string Mode { get; set; } = string.Empty;
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public List<TestGroup> TestGroups { get; set; } = new List<TestGroup>();
    }
}
