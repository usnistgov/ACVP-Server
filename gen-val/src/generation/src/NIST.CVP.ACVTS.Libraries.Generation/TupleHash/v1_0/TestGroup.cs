using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.TupleHash.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }

        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; }

        [JsonIgnore]
        public string Function { get; set; }

        [JsonProperty(PropertyName = "digestSize")]
        public int DigestSize { get; set; }

        [JsonProperty(PropertyName = "XOF")]
        public bool XOF { get; set; }

        [JsonIgnore]
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
