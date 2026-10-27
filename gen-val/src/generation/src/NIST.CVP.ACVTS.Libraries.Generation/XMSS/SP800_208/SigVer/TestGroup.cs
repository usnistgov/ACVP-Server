using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.SigVer.TestCaseExpectations;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.SigVer;

public class TestGroup : ITestGroup<TestGroup, TestCase>
{
    public int TestGroupId { get; set; }
    public string TestType { get; set; } = "AFT";

    public XmssMode XmssMode { get; set; }

    [JsonIgnore]
    public IXmssKeyPair KeyPair { get; set; }

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
    public readonly SignatureExpectationProvider TestCaseExpectationProvider = new();

    [JsonIgnore]
    public MathDomain MessageLength { get; set; }

    public List<TestCase> Tests { get; set; } = new();
}
