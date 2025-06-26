using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.AEAD128;

public class TestCase : ITestCase<TestGroup, TestCase>
{
    public int TestCaseId { get; set; }
    public bool? TestPassed { get; set; }
    public bool Deferred { get; set; }
    public TestGroup ParentGroup { get; set; }
    
    [JsonProperty(PropertyName = "key")]
    public BitString Key { get; set; }
    
    [JsonProperty(PropertyName = "nonce")]
    public BitString Nonce { get; set; }
    
    [JsonProperty(PropertyName = "aad")]
    public BitString AD { get; set; }
    
    [JsonProperty(PropertyName = "tag")]
    public BitString Tag { get; set; }
    
    [JsonProperty(PropertyName = "pt")]
    public BitString Plaintext { get; set; }
    
    [JsonProperty(PropertyName = "payloadLen")]
    public int PayloadBitLength { get; set; }
    
    [JsonProperty(PropertyName = "aadLen")]
    public int ADBitLength { get; set; }
    
    [JsonProperty(PropertyName = "tagLen")]
    public int TagLength { get; set; }
    
    [JsonProperty(PropertyName = "secondKey")]
    public BitString SecondKey { get; set; } = null;
    
    [JsonProperty(PropertyName = "ct")]
    public BitString Ciphertext { get; set; }
    
    [JsonProperty(PropertyName = "reason")]
    public AsconAEADDisposition Reason { get; set; }
}
