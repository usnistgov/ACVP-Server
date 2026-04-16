using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.SPDM;

public class TestCase : ITestCase<TestGroup, TestCase>
{
    public int TestCaseId { get; set; }
    public bool? TestPassed { get; set; }
    public bool Deferred { get; set; }
    public TestGroup ParentGroup { get; set; }
    
    public BitString Key { get; set; }

    [JsonProperty(PropertyName = "th1")]
    public BitString TH1 { get; set; }

    [JsonProperty(PropertyName = "th2")]
    public BitString TH2 { get; set; }
    
    public BitString RequestHandshakeSecret { get; set; }
    public BitString ResponseHandshakeSecret { get; set; }
    public BitString RequestDataSecret { get; set; }
    public BitString ResponseDataSecret { get; set; }
    public BitString ExportMasterSecret { get; set; }
}
