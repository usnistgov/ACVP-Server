using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.PBKDF
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }

        [JsonIgnore]
        public ShuffleQueue<int> KeyLength { get; set; }

        [JsonIgnore]
        public ShuffleQueue<int> PasswordLength { get; set; }

        [JsonIgnore]
        public ShuffleQueue<int> SaltLength { get; set; }

        [JsonIgnore]
        public ShuffleQueue<int> IterationCount { get; set; }

        [JsonIgnore]
        public HashFunction HashAlg { get; set; }

        [JsonIgnore] public int TestCasesForGroup { get; set; }

        [JsonProperty("hmacAlg")]
        public string HashAlgName => HashAlg.Name;

        public List<TestCase> Tests { get; set; } = new List<TestCase>();
    }
}
