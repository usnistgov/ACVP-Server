using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigVer;

public class TestGroup : ITestGroup<TestGroup, TestCase>
{
    public int TestGroupId { get; set; }
    public string TestType { get; set; }
    
    public DilithiumParameterSet ParameterSet { get; set; }
    public SignatureInterface SignatureInterface { get; init; }
    public PreHash PreHash { get; init; }
    public bool ExternalMu { get; init; }

    [JsonIgnore]
    public MathDomain ContextLength { get; init; }
    
    [JsonIgnore]
    public MathDomain MessageLength { get; init; }
    
    [JsonIgnore]
    public HashFunctions[] HashFunctions { get; init; }
    
    [JsonIgnore]
    public ITestCaseExpectationProvider<MLDSASignatureDisposition> TestCaseExpectationProvider { get; set; }
    
    public List<TestCase> Tests { get; set; } = new(); 
}
