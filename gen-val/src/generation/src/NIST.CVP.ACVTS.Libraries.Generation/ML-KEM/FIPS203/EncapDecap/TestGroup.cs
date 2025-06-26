using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Kyber;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap.TestCaseExpectations;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class TestGroup : ITestGroup<TestGroup, TestCase>
{
    public int TestGroupId { get; set; }
    public string TestType { get; set; }
    
    public KyberParameterSet ParameterSet { get; init; }
    public KyberFunction Function { get; set; }
    
    public List<TestCase> Tests { get; set; } = [];
    
    [JsonIgnore]
    public DecapsulationExpectationProvider DecapsulationExpectationProvider { get; init; }
    
    [JsonIgnore]
    public EncapsulationKeyExpectationProvider EncapsulationKeyExpectationProvider { get; init; }
    
    [JsonIgnore]
    public DecapsulationKeyExpectationProvider DecapsulationKeyExpectationProvider { get; init; }
}
