using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigVer;

public class TestCase : ITestCase<TestGroup, TestCase>
{
    public int TestCaseId { get; set; }
    
    public bool? TestPassed { get; set; }
    
    [JsonIgnore]
    public bool Deferred => false;
        
    public TestGroup ParentGroup { get; set; }

    public BitString Message { get; set; }
    public BitString Signature { get; set; }
    public LmsSignatureDisposition Reason { get; set; }
}
