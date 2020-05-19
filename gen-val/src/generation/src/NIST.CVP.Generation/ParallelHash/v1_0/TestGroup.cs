using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.ParallelHash.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }

        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; }

        public string Function { get; set; }

        [JsonProperty(PropertyName = "digestSize")]
        public int DigestSize { get; set; }
        
        [JsonProperty(PropertyName = "XOF")]
        public bool XOF { get; set; }
        
        public bool HexCustomization { get; set; } = false;

        [JsonIgnore]
        public MathDomain OutputLength { get; set; }
        
        [JsonProperty(PropertyName = "minOutLen")]
        public int MinOutputLength => OutputLength.GetDomainMinMax().Minimum;

        [JsonProperty(PropertyName = "maxOutLen")]
        public int MaxOutputLength => OutputLength.GetDomainMinMax().Maximum;

        [JsonIgnore]
        public MathDomain MessageLength { get; set; }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();
    }
}
