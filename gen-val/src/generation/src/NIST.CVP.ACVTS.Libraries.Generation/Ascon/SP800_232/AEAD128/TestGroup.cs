using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.AEAD128;

public class TestGroup : ITestGroup<TestGroup, TestCase>
{
    public int TestGroupId { get; set; }
    public string TestType { get; set; }
    public List<TestCase> Tests { get; set; } = new List<TestCase>();
    [JsonProperty(PropertyName = "direction")]
    public BlockCipherDirections Direction { get; set; }
    [JsonIgnore]
    public MathDomain PlaintextLength { get; set; }
    [JsonIgnore]
    public MathDomain ADLength { get; set; }
    [JsonIgnore]
    public MathDomain TruncationLength { get; set; }
    [JsonProperty(PropertyName = "supportsNonceMasking")]
    public bool NonceMasking { get; set; }
}

