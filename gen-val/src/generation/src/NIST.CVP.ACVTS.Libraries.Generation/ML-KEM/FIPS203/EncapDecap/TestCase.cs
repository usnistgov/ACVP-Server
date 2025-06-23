using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class TestCase : ITestCase<TestGroup, TestCase>
{
    public int TestCaseId { get; set; }
    public TestGroup ParentGroup { get; set; }
    public bool? TestPassed { get; set; }
    public bool Deferred { get; }
    
    [JsonProperty(PropertyName = "ek")]
    public BitString EncapsulationKey { get; set; }

    [JsonProperty(PropertyName = "dk")]
    public BitString DecapsulationKey { get; set; }
    
    [JsonProperty(PropertyName = "c")]
    public BitString Ciphertext { get; set; }
    
    [JsonProperty(PropertyName = "k")]
    public BitString SharedKey { get; set; }
    
    [JsonProperty(PropertyName = "m")]
    public BitString SeedM { get; set; }
    
    // This is set up to enable previous internalProjections because the property is now a generic string.
    // This may need to be a string permanently because of the multiple enums supported through the TestCaseExpectations 
    public string Reason { get; set; }
}
