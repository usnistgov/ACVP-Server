using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Kyber;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class TestGroup : ITestGroup<TestGroup, TestCase>
{
    public int TestGroupId { get; set; }
    public string TestType { get; set; }
    
    public KyberParameterSet ParameterSet { get; set; }
    
    public KyberFunction Function { get; set; }
    
    [JsonProperty(PropertyName = "ek")]
    public BitString EncapsulationKey { get; set; }
    
    [JsonProperty(PropertyName = "dk")]
    public BitString DecapsulationKey { get; set; }
    
    public List<TestCase> Tests { get; set; } = new();
    
    [JsonIgnore]
    public ITestCaseExpectationProvider<MLKEMDecapsulationDisposition> TestCaseExpectationProvider { get; set; }
}
