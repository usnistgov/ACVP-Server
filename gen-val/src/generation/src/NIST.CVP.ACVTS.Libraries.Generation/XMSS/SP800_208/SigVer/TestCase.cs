using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.SigVer;

public class TestCase : ITestCase<TestGroup, TestCase>
{
    public int TestCaseId { get; set; }

    public bool? TestPassed { get; set; }

    [JsonIgnore]
    public bool Deferred => false;

    public TestGroup ParentGroup { get; set; }

    public int MessageLength { get; set; }
    public BitString Message { get; set; }
    public BitString Signature { get; set; }
    public XmssSignatureDisposition Reason { get; set; }
}
