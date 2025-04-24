using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.CXOF128;

public class TestCase : ITestCase<TestGroup, TestCase>
{
    public int TestCaseId { get; set; }
    [JsonIgnore]
    public bool? TestPassed => true;
    public bool Deferred { get; set; }
    public TestGroup ParentGroup { get; set; }
    [JsonProperty(PropertyName = "message")]
    public BitString Message { get; set; }
    [JsonProperty(PropertyName = "messageBitLength")]
    public int MessageBitLength { get; set; }
    [JsonProperty(PropertyName = "digest")]
    public BitString Digest { get; set; }
    [JsonProperty(PropertyName = "digestBitLength")]
    public int DigestBitLength { get; set; }
    [JsonProperty(PropertyName = "customizationString")]
    public BitString CS { get; set; }
    [JsonProperty(PropertyName = "customizationStringBitLength")]
    public int CSBitLength { get; set; }
    
}

