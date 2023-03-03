using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigGen;

public class TestCase : ITestCase<TestGroup, TestCase>
{
    public int TestCaseId { get; set; }
    
    [JsonIgnore]
    public bool? TestPassed => true;
    
    [JsonIgnore]
    public bool Deferred => false;
        
    public TestGroup ParentGroup { get; set; }

    public BitString Message { get; set; }
    public BitString Signature { get; set; }
}
