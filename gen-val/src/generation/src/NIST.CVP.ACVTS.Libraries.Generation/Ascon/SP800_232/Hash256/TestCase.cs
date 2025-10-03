using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.Hash256;

public class TestCase : ITestCase<TestGroup, TestCase>
{
    public int TestCaseId { get; set; }
    public bool? TestPassed { get; set; }
    public bool Deferred { get; set; }
    public TestGroup ParentGroup { get; set; }
    
    [JsonProperty(PropertyName = "msg")]
    public BitString Message { get; set; }
    
    [JsonProperty(PropertyName = "len")]
    public int MessageBitLength { get; set; }
    
    [JsonProperty(PropertyName = "md")]
    public BitString Digest { get; set; }
}
