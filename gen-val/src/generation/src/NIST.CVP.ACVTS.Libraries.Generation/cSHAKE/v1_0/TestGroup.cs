using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.cSHAKE.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }

        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; }

        [JsonIgnore]
        public string Function { get; set; }

        [JsonIgnore]
        public int DigestSize { get; set; }

        [JsonProperty(PropertyName = "hexCustomization")]
        public bool HexCustomization { get; set; } = false;

        [JsonIgnore]
        public MathDomain OutputLength { get; set; }

        [JsonProperty(PropertyName = "minOutLen")]
        public int MinOutputLength => OutputLength.GetDomainMinMax().Minimum;

        [JsonProperty(PropertyName = "maxOutLen")]
        public int MaxOutputLength => OutputLength.GetDomainMinMax().Maximum;

        [JsonProperty(PropertyName = "outLenIncrement")]
        public int OutLenIncrement => OutputLength.GetDomainMinMax().Increment;
        
        [JsonIgnore]
        public MathDomain MessageLength { get; set; }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();
    }
}
