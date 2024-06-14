using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.SigGen;

public class TestCase : ITestCase<TestGroup, TestCase>
{
    public int TestCaseId { get; set; }
    public TestGroup ParentGroup { get; set; }
    public bool? TestPassed { get; }
    public bool Deferred { get; }
    
    [JsonProperty(PropertyName = "sk")]
    public BitString PrivateKey { get; set; }
    public BitString AdditionalRandomness { get; set; }
    public int MessageLength { get; set; }
    public BitString Message { get; set; }
    
    public BitString Signature { get; set; }
}
