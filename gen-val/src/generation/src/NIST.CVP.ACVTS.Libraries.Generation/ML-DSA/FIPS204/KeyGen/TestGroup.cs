using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.KeyGen;

public class TestGroup : ITestGroup<TestGroup, TestCase>
{
    public int TestGroupId { get; set; }
    public string TestType { get; set; }
    
    public DilithiumParameterSet ParameterSet { get; set; }
    
    public List<TestCase> Tests { get; set; } = new();
}
