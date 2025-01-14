using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.SigVer;

public class TestGroup : ITestGroup<TestGroup, TestCase>
{
    public int TestGroupId { get; set; }
    public string TestType { get; set; }
    public SlhdsaParameterSet ParameterSet { get; set; }
    public SignatureInterface SignatureInterface { get; init; }
    public PreHash PreHash { get; init; }
    
    [JsonIgnore]
    public HashFunctions[] HashFunctions { get; init; }
    
    [JsonIgnore]
    public MathDomain ContextLength { get; init; }
    
    [JsonIgnore]
    public MathDomain MessageLength { get; set; }
    
    [JsonIgnore]
    public ITestCaseExpectationProvider<SLHDSASignatureDisposition> TestCaseExpectationProvider { get; set; }
    
    public List<TestCase> Tests { get; set; } = new();
}
