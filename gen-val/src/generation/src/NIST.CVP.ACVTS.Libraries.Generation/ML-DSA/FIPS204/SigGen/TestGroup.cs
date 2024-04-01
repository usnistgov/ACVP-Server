using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;

public class TestGroup : ITestGroup<TestGroup, TestCase>
{
    public int TestGroupId { get; set; }
    public string TestType { get; set; }
    
    public DilithiumParameterSet ParameterSet { get; set; }
    public bool Deterministic { get; set; }
    
    [JsonProperty(PropertyName = "pk")]
    public BitString PublicKey { get; set; }

    [JsonProperty(PropertyName = "sk")]
    public BitString PrivateKey { get; set; }
    
    public List<TestCase> Tests { get; set; } = new();
}
