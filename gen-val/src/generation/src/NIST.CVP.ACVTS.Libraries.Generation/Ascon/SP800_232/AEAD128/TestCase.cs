using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.AEAD128;

public class TestCase : ITestCase<TestGroup, TestCase>
{
    public int TestCaseId { get; set; }
    [JsonIgnore]
    public bool? TestPassed => true;
    public bool Deferred { get; set; }
    public TestGroup ParentGroup { get; set; }
    [JsonProperty(PropertyName = "key")]
    public BitString Key { get; set; }
    [JsonProperty(PropertyName = "nonce")]
    public BitString Nonce { get; set; }
    [JsonProperty(PropertyName = "ad")]
    public BitString AD { get; set; }
    [JsonProperty(PropertyName = "tag")]
    public BitString Tag { get; set; }
    [JsonProperty(PropertyName = "pt")]
    public BitString Plaintext { get; set; }
    [JsonProperty(PropertyName = "payloadBitLength")]
    public int PayloadBitLength { get; set; }
    [JsonProperty(PropertyName = "adBitLength")]
    public int ADBitLength { get; set; }
    [JsonProperty(PropertyName = "tagLength")]
    public int TagLength { get; set; }
    [JsonProperty(PropertyName = "secondKey")]
    public BitString SecondKey { get; set; } = null;
    [JsonProperty(PropertyName = "ct")]
    public BitString Ciphertext { get; set; }
}

