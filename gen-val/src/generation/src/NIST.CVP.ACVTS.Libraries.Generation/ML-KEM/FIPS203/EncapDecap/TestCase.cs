using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class TestCase : ITestCase<TestGroup, TestCase>
{
    public int TestCaseId { get; set; }
    public TestGroup ParentGroup { get; set; }
    public bool? TestPassed { get; }
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
    
    public MLKEMDecapsulationDisposition Reason { get; set; }
}
