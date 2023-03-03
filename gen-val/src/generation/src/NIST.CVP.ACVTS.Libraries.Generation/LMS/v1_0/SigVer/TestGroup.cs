using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigVer.TestCaseExpectations;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigVer;

public class TestGroup : ITestGroup<TestGroup, TestCase>
{
    public int TestGroupId { get; set; }
    public string TestType { get; set; } = "AFT";

    public LmsMode LmsMode { get; set; }
    public LmOtsMode LmOtsMode { get; set; }
    
    [JsonIgnore]
    public ILmsKeyPair KeyPair { get; set; }
    
    // Only used for testing by TestDataMother
    [JsonIgnore]
    private BitString TestingPublicKey { get; init; }

    public BitString PublicKey
    {
        get
        {
            return KeyPair != null ? new BitString(KeyPair.PublicKey.Key) : TestingPublicKey;
        }

        init
        {
            TestingPublicKey = value;
        }
    }
    
    [JsonIgnore]
    public readonly ITestCaseExpectationProvider<LmsSignatureDisposition> TestCaseExpectationProvider = new TestCaseExpectationProvider();

    public List<TestCase> Tests { get; set; } = new();
}
