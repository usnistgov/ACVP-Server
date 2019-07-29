using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KDF_Components.v1_0.PBKDF
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }

        [JsonIgnore]
        public MathDomain KeyLength { get; set; }
        
        [JsonIgnore]
        public MathDomain PasswordLength { get; set; }
        
        [JsonIgnore]
        public MathDomain SaltLength { get; set; }
        
        [JsonIgnore]
        public MathDomain IterationCount { get; set; }
        
        [JsonIgnore]
        public HashFunction HashAlg { get; set; }

        [JsonProperty("hmacAlg")]
        public string HashAlgName => HashAlg.Name;
        
        public List<TestCase> Tests { get; set; } = new List<TestCase>();
    }
}