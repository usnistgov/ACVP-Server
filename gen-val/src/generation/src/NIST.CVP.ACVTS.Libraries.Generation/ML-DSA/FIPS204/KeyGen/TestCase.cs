using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.KeyGen;

public class TestCase : ITestCase<TestGroup, TestCase>
{
    public int TestCaseId { get; set; }
    public TestGroup ParentGroup { get; set; }
    public bool? TestPassed { get; }
    public bool Deferred { get; }
    
    public BitString Seed { get; set; }
    
    [JsonProperty(PropertyName = "pk")]
    public BitString PublicKey { get; set; }
    
    [JsonProperty(PropertyName = "sk")]
    public BitString PrivateKey { get; set; }
}
