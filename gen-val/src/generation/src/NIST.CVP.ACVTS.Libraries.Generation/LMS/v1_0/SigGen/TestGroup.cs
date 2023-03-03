using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigGen;

public class TestGroup : ITestGroup<TestGroup, TestCase>
{
    public int TestGroupId { get; set; }
    public string TestType { get; set; }

    public LmsMode LmsMode { get; set; }
    public LmOtsMode LmOtsMode { get; set; }
    
    [JsonIgnore]
    public ILmsKeyPair KeyPair { get; set; }
    public BitString PublicKey { get; set; }
    
    public List<TestCase> Tests { get; set; } = new();
}
