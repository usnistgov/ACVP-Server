using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.KeyGen;

public class TestCase : ITestCase<TestGroup, TestCase>
{
    public int TestCaseId { get; set; }
    public TestGroup ParentGroup { get; set; }
    public bool? TestPassed { get; }
    public bool Deferred { get; }
    
    [JsonProperty(PropertyName = "z")]
    public BitString SeedZ { get; set; }
    
    [JsonProperty(PropertyName = "d")]
    public BitString SeedD { get; set; }
    
    [JsonProperty(PropertyName = "ek")]
    public BitString EncapsulationKey { get; set; }
    
    [JsonProperty(PropertyName = "dk")]
    public BitString DecapsulationKey { get; set; }
}
