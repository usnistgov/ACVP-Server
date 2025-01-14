using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigVer;

public class TestCase : ITestCase<TestGroup, TestCase>
{
    public int TestCaseId { get; set; }
    public TestGroup ParentGroup { get; set; }
    public bool? TestPassed { get; set; }
    public bool Deferred { get; }
    
    [JsonProperty(PropertyName = "pk")]
    public BitString PublicKey { get; set; }

    [JsonProperty(PropertyName = "sk")]
    public BitString PrivateKey { get; set; }
    
    public BitString Message { get; set; }
    public BitString Mu { get; set; }
    public BitString Context { get; set; }
    public HashFunctions HashAlg { get; set; }
    public BitString Signature { get; set; }
    public MLDSASignatureDisposition Reason { get; set; }
}
