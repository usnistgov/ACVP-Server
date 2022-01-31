using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.HKDF.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        [JsonIgnore]
        public HashFunction HmacAlg { get; set; }

        [JsonProperty(PropertyName = "hmacAlg")]
        public string HmacAlgName => HmacAlg.Name;

        [JsonIgnore]
        public MathDomain KeyLength { get; set; }

        [JsonIgnore]
        public MathDomain SaltLength { get; set; }

        [JsonIgnore]
        public MathDomain InputKeyingMaterialLength { get; set; }

        [JsonIgnore]
        public MathDomain OtherInfoLength { get; set; }
    }
}
